using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NUglify.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WorldTravel.Abstract;
using WorldTravel.Dtos.MessageContents;
using WorldTravel.Dtos.MessageContents.ViewModels;
using WorldTravel.Dtos.Messages;
using WorldTravel.Dtos.Messages.ViewModels;
using WorldTravel.Entities.MessageContents;
using WorldTravel.Entities.Messages;
using WorldTravel.Enums;
using WorldTravel.Extensions;
using WorldTravel.Localization;
using WorldTravel.Models.Results.Abstract;
using WorldTravel.Models.Results.Concrete;
using WorldTravel.Permissions;

namespace WorldTravel.Services
{
    [Authorize(WorldTravelPermissions.Message.Default)]
    public class MessageAppService : CrudAppService<Message, MessageDto, int, PagedAndSortedResultRequestDto, MessageDto, MessageDto>, IMessageAppService
    {
        private readonly IFileAppService _fileAppService;
        private readonly IStringLocalizer<WorldTravelResource> _L;
        private readonly IRepository<MessageContent, int> _messageContentRepository;

        public MessageAppService(
            IRepository<Message, int> repository,
            IFileAppService fileAppService,
            IStringLocalizer<WorldTravelResource> L,
            IRepository<MessageContent, int> messageContentRepository
            ) : base(repository)
        {
            _fileAppService = fileAppService;
            _L = L;
            _messageContentRepository = messageContentRepository;
        }


        /// <summary>
        /// 1) Mesajlaşılan kişileri getirir. Kişiler bölümü
        /// </summary>
        /// <returns></returns>
        [Authorize(WorldTravelPermissions.Message.See)]
        public async Task<IDataResult<List<MessageViewModel>>> GetUserMessageListAsync()
        {
            try
            {
                //if (!CurrentUser.IsAuthenticated) //KONTROL ET
                //    return new ErrorDataResult<List<MessageViewModel>>(_L["YouMustLogin"].Value);

                var currentUserId = CurrentUser.Id.Value;

                //var entity = await Repository.FirstOrDefaultAsync(x => x.Status == Status.Active && (x.SenderId == currentUserId || x.ReceiverId == currentUserId));
                //if (entity == null)
                //    return new ErrorDataResult<List<MessageViewModel>>(_L["EntityNotFoundError"].Value);

                //var isSender = entity.SenderId == currentUserId;

                // var isAllowed = Repository.Any(x => x.Status == Status.Active && x.Id == messageId &&
                //(isSender == true ? (x.SenderId == currentUserId && x.ReceiverId == toUserId) : (x.SenderId == toUserId && x.ReceiverId == currentUserId)));
                // if (!isAllowed)
                //     return new ErrorDataResult<List<MessageViewModel>>();

                var query = Repository
                    .Where(x => x.Status == Status.Active &&
                    (x.SenderId == currentUserId ?
                    (x.SenderId == currentUserId && x.SenderStatus != MessageStatus.Deleted) :
                    (x.ReceiverId == currentUserId && x.ReceiverStatus != MessageStatus.Deleted)))
                    //(x.SenderId == currentUserId || x.ReceiverId == currentUserId))
                    .Include(x => x.MessageContents)
                    .AsQueryable();

                query = query.Include(x => x.Sender).Include(x => x.Receiver);

                var list = await AsyncExecuter.ToListAsync(query);
                var viewModels = list
                    .Select(x =>
                    {
                        var isSender = x.SenderId == currentUserId;
                        var viewModel = ObjectMapper.Map<Message, MessageViewModel>(x);
                        var shownUser = isSender ? x.Receiver : x.Sender;
                        var isUserRole = (shownUser.UserType != null && shownUser.UserType == UserType.User);

                        viewModel.ShownUserFullName = shownUser.Name + " " + shownUser.Surname;
                        viewModel.ShownUserImageUrl = _fileAppService.SetDefaultImageIfFileIsNull(shownUser.ImageId, shownUser.Gender, !isUserRole);
                        viewModel.ShownUserZodiacSign = shownUser.BirthDate.HasValue ? shownUser.BirthDate.Value.ToZodiacSign() : "";
                        viewModel.UnSeenMessageCount = x.MessageContents.Where(x => x.ReceiverId == currentUserId && x.IsSeen == false && x.IsDeletedForReceiver == false).Count();
                        viewModel.LastMessageDate = x.MessageContents.LastOrDefault().CreatedDate;
                        //Son görülme tarihi aslında herhangi bir kişiye yazdığı son yazının tarihi bu guncellenecek
                        //kişilerden herhangi birine tıklayıp mesaj açtığında olacak.
                        if (_messageContentRepository.Any(x => x.SenderId == shownUser.Id))
                        {
                            var isExist = _messageContentRepository.Where(x => x.SenderId == shownUser.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                            viewModel.ShownUserLastSeenDate = isExist.CreatedDate.ToMessageSendDateString(isUserLastSeen: true);
                        }
                        else
                        {
                            viewModel.ShownUserLastSeenDate = "-";
                        }

                        //Kişiler bölümündeki engelle ikonunu getirir.
                        viewModel.IsBlocked = (isSender ? (x.SenderStatus == MessageStatus.Blocked) : (x.ReceiverStatus == MessageStatus.Blocked));
                        viewModel.IsMuted = (isSender ? (x.SenderStatus == MessageStatus.Muted) : (x.ReceiverStatus == MessageStatus.Muted));
                        return viewModel;
                    })
                    .OrderByDescending(x => x.LastMessageDate)
                    .ToList();

                return new SuccessDataResult<List<MessageViewModel>>(viewModels);

            }
            catch (Exception ex)
            {
                Log.Error(ex, "MessageAppService > GetUserMessageListAsync has error! ");

                return new ErrorDataResult<List<MessageViewModel>>(L["GeneralError"]);
            }
        }


        /// <summary>
        /// Mesajlaşılan kişiler arasındaki seçilen kişiyle yapılan konuşmalayı ; message content leri getirir. 
        /// Yani mesajlaşma geçmişini getirir.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        [Authorize(WorldTravelPermissions.Message.See)]
        public async Task<IDataResult<MessageWithContentViewModel>> GetUserMessageWithContentListAsync(int messageId)
        {
            try
            {
                if (!CurrentUser.IsAuthenticated)
                    return new ErrorDataResult<MessageWithContentViewModel>(_L["YouMustLogin"].Value);
                var currentUserId = CurrentUser.Id.Value;

                var entity = await Repository.FirstOrDefaultAsync(x => x.Status == Status.Active && x.Id == messageId);
                if (entity == null)
                    return new ErrorDataResult<MessageWithContentViewModel>(_L["EntityNotFoundError"].Value);

                var isSender = entity.SenderId == currentUserId;

                var isAllowed = Repository.Any(x => x.Status == Status.Active && x.Id == messageId &&
                (isSender == true ? x.SenderId == currentUserId : x.ReceiverId == currentUserId));
                if (!isAllowed)
                    return new ErrorDataResult<MessageWithContentViewModel>(_L["NotAllowedProcess"].Value);

                var entityIncludes = await Repository
                    .Where(x => x.Status == Status.Active && x.Id == messageId)
                    .Include(x => x.Sender).Include(x => x.Receiver)
                    .Include(x => x.MessageContents)
                    .FirstOrDefaultAsync();

                MessageWithContentViewModel viewModel = new MessageWithContentViewModel();
                viewModel.MessageId = entityIncludes.Id;
                var shownUser = isSender ? entityIncludes.Receiver : entityIncludes.Sender;
                viewModel.ShownUserId = shownUser.Id;
                viewModel.ShownUserFullName = shownUser.Name + " " + shownUser.Surname;
                var isUserRole = (shownUser.UserType != null && shownUser.UserType == UserType.User);
                var shownUserImageUrl = _fileAppService.SetDefaultImageIfFileIsNull(shownUser.ImageId, shownUser.Gender, !isUserRole);
                viewModel.ShownUserImageUrl = shownUserImageUrl;

                //Mesaj işlemlerinde ki Engelle butonunun durumu için kullanılıyor.
                viewModel.IsBlocked = (isSender ? (entityIncludes.SenderStatus == MessageStatus.Blocked) : (entityIncludes.ReceiverStatus == MessageStatus.Blocked));
                viewModel.IsMuted = (isSender ? (entityIncludes.SenderStatus == MessageStatus.Muted) : (entityIncludes.ReceiverStatus == MessageStatus.Muted));

                //Kendi sildiği mesaj görmesin 
                var messageContents = entityIncludes.MessageContents
                    .Where(x => (x.SenderId == currentUserId ? x.IsDeletedForSender == false : x.IsDeletedForReceiver == false))
                    .OrderBy(x => x.CreatedDate);

                foreach (var messagecontent in messageContents)
                {
                    var messageContentViewModel = ObjectMapper.Map<MessageContent, MessageContentViewModel>(messagecontent);
                    messageContentViewModel.IsMine = messagecontent.SenderId == currentUserId;
                    messageContentViewModel.MessageSendDate = messagecontent.CreatedDate.ToMessageSendDateString();
                    //messageContentViewModel.IsTodayDivider
                    viewModel.MessageContents.Add(messageContentViewModel);
                }

                //Okundu olarak işaretle.
                var unseenMessageContent = entityIncludes.MessageContents.Where(x => x.ReceiverId == currentUserId && x.IsSeen == false).ToList();
                if (unseenMessageContent.Count > 0)
                {
                    unseenMessageContent.ForEach(x => { x.IsSeen = true; });
                    await _messageContentRepository.UpdateManyAsync(unseenMessageContent, true);
                }

                return new SuccessDataResult<MessageWithContentViewModel>(viewModel);

            }
            catch (Exception ex)
            {
                Log.Error(ex, "MessageAppService > GetUserMessageWithContentListAsync has error! ");

                return new ErrorDataResult<MessageWithContentViewModel>(_L["GeneralError"].Value);
            }
        }


        /// <summary>
        /// Kendi gönderdiği mesaj contenti yada karşıdaki gönderen kişinin mesaj contentini, KENDI profilinde silmesine yarar.
        /// </summary>
        /// <param name="messageContentId"></param>
        /// <returns></returns>
        [Authorize(WorldTravelPermissions.Message.See)]
        [IgnoreAntiforgeryToken]
        public async Task<IDataResult<string>> DeleteMessageContentAsync(int messageContentId)
        {
            try
            {
                if (!CurrentUser.IsAuthenticated)
                    return new ErrorDataResult<string>(_L["YouMustLogin"].Value);

                var currentUserId = CurrentUser.Id.Value;

                var entity = await _messageContentRepository.FirstOrDefaultAsync(x => x.Status == Status.Active && x.Id == messageContentId);
                if (entity == null)
                    return new ErrorDataResult<string>(_L["EntityNotFoundError"].Value);

                var isSender = entity.SenderId == currentUserId;

                var allowedEntity = await _messageContentRepository.FirstOrDefaultAsync(x => x.Status == Status.Active && x.Id == messageContentId &&
                (isSender == true ? x.SenderId == currentUserId : x.ReceiverId == currentUserId));
                if (allowedEntity == null)
                    return new ErrorDataResult<string>(_L["NotAllowedProcess"].Value);

                if (isSender)
                {
                    allowedEntity.IsDeletedForSender = true;
                    allowedEntity.DeletedDateForSender = DateTime.Now;
                }
                else
                {
                    entity.IsDeletedForReceiver = true;
                    allowedEntity.DeletedDateForReceived = DateTime.Now;
                }

                await _messageContentRepository.UpdateAsync(allowedEntity, true);

                return new SuccessDataResult<string>(_L["MessageSuccessfullyDeleted"].Value);

            }
            catch (Exception ex)
            {
                Log.Error(ex, "MessageAppService > DeleteMessageContentAsync has error! ");

                return new ErrorDataResult<string>(_L["GeneralError"].Value);
            }
        }


        /// <summary>
        /// Mesaj göndermeye yarar. (MessageContent insert işlemi yapar)
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="toUserId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [Authorize(WorldTravelPermissions.Message.See)]
        [IgnoreAntiforgeryToken]
        public async Task<IDataResult<MessageContentViewModel>> InsertMessageContentAsync(int messageId, Guid toUserId, string content)
        {
            try
            {
                if (!CurrentUser.IsAuthenticated)
                    return new ErrorDataResult<MessageContentViewModel>(_L["YouMustLogin"].Value);
                var currentUserId = CurrentUser.Id.Value;

                var entity = await Repository.FirstOrDefaultAsync(x => x.Status == Status.Active && x.Id == messageId);
                if (entity == null)
                    return new ErrorDataResult<MessageContentViewModel>(_L["EntityNotFoundError"].Value);

                var isSender = entity.SenderId == currentUserId;

                var allowedEntity = await Repository.FirstOrDefaultAsync(x => x.Status == Status.Active && x.Id == messageId &&
               (isSender == true ? (x.SenderId == currentUserId && x.ReceiverId == toUserId) : (x.SenderId == toUserId && x.ReceiverId == currentUserId)));
                if (allowedEntity == null)
                    return new ErrorDataResult<MessageContentViewModel>(_L["NotAllowedProcess"].Value);

                //Karşı kişi, current useri  blokladıysa , Current user mesaj gönderemez..
                var isBlocked = isSender ? (allowedEntity.ReceiverStatus == MessageStatus.Blocked) : ((allowedEntity.SenderStatus == MessageStatus.Blocked));
                if (isBlocked)
                    return new ErrorDataResult<MessageContentViewModel>(_L["BlockedMessage"].Value);

                //Blokladığım kişiye mesaj gönderiyorsam
                var isBlockedByMe = isSender ? (allowedEntity.SenderStatus == MessageStatus.Blocked) : ((allowedEntity.ReceiverStatus == MessageStatus.Blocked));
                if (isBlockedByMe)
                {
                    allowedEntity.ReceiverStatus = MessageStatus.Default;
                    allowedEntity.ReceiverStatusDate = DateTime.Now;
                    await Repository.UpdateAsync(allowedEntity, true);
                }

                //Karşı kişi, current userin mesajını sildiyse , current user mesaj attığında mesajStatus defaulta çekilir.
                var isDeleted = isSender ? (allowedEntity.ReceiverStatus == MessageStatus.Deleted) : ((allowedEntity.SenderStatus == MessageStatus.Deleted));
                if (isDeleted)
                {
                    if (isSender)
                    {
                        allowedEntity.ReceiverStatus = MessageStatus.Default;
                        allowedEntity.ReceiverStatusDate = DateTime.Now;
                    }
                    else
                    {
                        allowedEntity.SenderStatus = MessageStatus.Default;
                        allowedEntity.SenderStatusDate = DateTime.Now;
                    }
                    await Repository.UpdateAsync(allowedEntity, true);
                }

                CreateUpdateMessageContentDto messageContentDto = new CreateUpdateMessageContentDto();
                messageContentDto.MessageId = messageId;
                messageContentDto.Content = content;
                messageContentDto.SenderId = currentUserId;
                messageContentDto.ReceiverId = toUserId;

                var messageContent = ObjectMapper.Map<CreateUpdateMessageContentDto, MessageContent>(messageContentDto);

                var insertedMessageContent = await _messageContentRepository.InsertAsync(messageContent, true);

                var messageContentViewModel = ObjectMapper.Map<MessageContent, MessageContentViewModel>(insertedMessageContent);
                messageContentViewModel.IsMine = true;
                messageContentViewModel.MessageSendDate = insertedMessageContent.CreatedDate.ToMessageSendDateString();

                return new SuccessDataResult<MessageContentViewModel>(messageContentViewModel);

            }
            catch (Exception ex)
            {
                Log.Error(ex, "MessageAppService > InsertMessageContentAsync has error! ");

                return new ErrorDataResult<MessageContentViewModel>(_L["GeneralError"].Value);
            }
        }


        /// <summary>
        ///  Kişiyi engelleme,bloklamaya yada mesajı silmeye yarar.
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="toUserId"></param>
        /// <param name="messageStatus"></param>
        /// <returns></returns>
        [Authorize(WorldTravelPermissions.Message.See)]
        [IgnoreAntiforgeryToken]
        public async Task<IDataResult<string>> ChangeMessageStatusAsync(int messageId, Guid toUserId, MessageStatus messageStatus)
        {
            try
            {
                if (!CurrentUser.IsAuthenticated)
                    return new ErrorDataResult<string>(_L["YouMustLogin"].Value);

                var currentUserId = CurrentUser.Id.Value;

                var entity = await Repository.FirstOrDefaultAsync(x => x.Status == Status.Active && x.Id == messageId);
                if (entity == null)
                    return new ErrorDataResult<string>(_L["EntityNotFoundError"].Value);

                var isSender = entity.SenderId == currentUserId;

                var allowedEntity = await Repository
                    .Where(x => x.Status == Status.Active && x.Id == messageId &&
                (isSender == true ? (x.SenderId == currentUserId && x.ReceiverId == toUserId) : (x.SenderId == toUserId && x.ReceiverId == currentUserId)))
                    .Include(x => x.MessageContents)
                    .FirstOrDefaultAsync();
                if (allowedEntity == null)
                    return new ErrorDataResult<string>(_L["NotAllowedProcess"].Value);

                if (isSender)
                    allowedEntity.SenderStatus = messageStatus;
                else
                    allowedEntity.ReceiverStatus = messageStatus;

                //Geçmiş mesajların görünürlüğünü kapatıyor.
                if (messageStatus == MessageStatus.Deleted)
                {
                    allowedEntity.MessageContents
                    .Where(x => x.SenderId == currentUserId)
                           .ForEach(x => { x.IsDeletedForSender = true; x.DeletedDateForSender = DateTime.Now; });
                    allowedEntity.MessageContents
                         .Where(x => x.ReceiverId == currentUserId)
                         .ForEach(x => { x.IsDeletedForReceiver = true; x.DeletedDateForReceived = DateTime.Now; });
                }

                await Repository.UpdateAsync(allowedEntity, true);

                return new SuccessDataResult<string>(_L["SuccessfullyCompleted"].Value);

            }
            catch (Exception ex)
            {
                Log.Error(ex, "MessageAppService > ChangeMessageStatusAsync has error! ");

                return new ErrorDataResult<string>(_L["GeneralError"].Value);
            }
        }


        /// <summary>
        /// Kullanıcının mesaj sayısını getirir. Menüde ki mesaj bölümüne basılıyor.
        /// </summary>
        /// <returns></returns>
        [Authorize(WorldTravelPermissions.Message.See)]
        public int GetUnseenMessageCount()
        {
            var result = 0;
            try
            {
                if (!CurrentUser.IsAuthenticated)
                    return result;

                var currentUserId = CurrentUser.Id.Value;

                return _messageContentRepository.Include(x => x.Message)
                      .Where(x => x.ReceiverId == currentUserId && x.IsSeen == false && x.IsDeletedForReceiver == false && x.Message.ReceiverStatus != MessageStatus.Blocked && x.Message.ReceiverStatus != MessageStatus.Muted)
                      .GroupBy(x => x.MessageId)
                      .Count();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "MessageAppService > GetUnseenMessageCount has error! ");

                return result;
            }
        }

        /// <summary>
        /// Mesaj göndermeye yarar. (MessageContent insert işlemi yapar)
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="toUserId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [Authorize(WorldTravelPermissions.Message.See)]
        [IgnoreAntiforgeryToken]
        public async Task<IDataResult<string>> SendMessageAsync(Guid toUserId, string content)
        {
            try
            {
                if (!CurrentUser.IsAuthenticated)
                    return new ErrorDataResult<string>(_L["YouMustLogin"].Value);
                var currentUserId = CurrentUser.Id.Value;

                //var entity = await Repository.FirstOrDefaultAsync(x => x.Status == Status.Active && x.Id == messageId);
                //if (entity == null)
                //    return new ErrorDataResult<MessageContentViewModel>(_L["EntityNotFoundError"].Value);

                //var isSender = entity.SenderId == currentUserId;

                // TO yada Receiverı  currentUser   VE YA  To yada receiver toUserId olan bir mesaj var mı?
                var isExist = await Repository.FirstOrDefaultAsync(x => x.Status == Status.Active && (x.SenderId == currentUserId && x.ReceiverId == toUserId) || (x.SenderId == toUserId && x.ReceiverId == currentUserId));
                if (isExist != null)
                {
                    var isSender = isExist.SenderId == currentUserId;

                    //Karşı kişi, current useri  blokladıysa , Current user mesaj gönderemez..
                    var isBlocked = isSender ? (isExist.ReceiverStatus == MessageStatus.Blocked) : (isExist.SenderStatus == MessageStatus.Blocked);
                    if (isBlocked)
                        return new ErrorDataResult<string>(_L["BlockedMessage"].Value);

                    //Blokladığım kişiye mesaj gönderiyorsam
                    var isBlockedByMe = isSender ? (isExist.SenderStatus == MessageStatus.Blocked) : (isExist.ReceiverStatus == MessageStatus.Blocked);
                    if (isBlockedByMe)
                    {
                        isExist.ReceiverStatus = MessageStatus.Default;
                        isExist.ReceiverStatusDate = DateTime.Now;
                        await Repository.UpdateAsync(isExist, true);
                    }

                    //Karşı kişi, current userin mesajını sildiyse , current user mesaj attığında mesajStatus defaulta çekilir.
                    var isDeleted = isSender ? (isExist.ReceiverStatus == MessageStatus.Deleted) : (isExist.SenderStatus == MessageStatus.Deleted);
                    if (isDeleted)
                    {
                        if (isSender)
                        {
                            isExist.ReceiverStatus = MessageStatus.Default;
                            isExist.ReceiverStatusDate = DateTime.Now;
                        }
                        else
                        {
                            isExist.SenderStatus = MessageStatus.Default;
                            isExist.SenderStatusDate = DateTime.Now;
                        }
                        await Repository.UpdateAsync(isExist, true);

                    }

                    CreateUpdateMessageContentDto messageContentDto = new CreateUpdateMessageContentDto();
                    messageContentDto.MessageId = isExist.Id;
                    messageContentDto.Content = content;
                    if (isSender)
                    {
                        messageContentDto.SenderId = currentUserId;
                        messageContentDto.ReceiverId = toUserId;
                    }
                    else
                    {
                        messageContentDto.SenderId = toUserId;
                        messageContentDto.ReceiverId = currentUserId;
                    }

                    var messageContent = ObjectMapper.Map<CreateUpdateMessageContentDto, MessageContent>(messageContentDto);
                    var insertedMessageContent = await _messageContentRepository.InsertAsync(messageContent, true);

                    return new SuccessDataResult<string>(_L["SuccessfullyCompleted"].Value);
                }
                else
                {
                    CreateUpdateMessageDto messageDto = new CreateUpdateMessageDto();
                    messageDto.SenderId = currentUserId;
                    messageDto.ReceiverId = toUserId;

                    var message = ObjectMapper.Map<CreateUpdateMessageDto, Message>(messageDto);
                    var insertedMessage = await Repository.InsertAsync(message, true);

                    CreateUpdateMessageContentDto messageContentDto = new CreateUpdateMessageContentDto();
                    messageContentDto.MessageId = insertedMessage.Id;
                    messageContentDto.Content = content;
                    messageContentDto.SenderId = currentUserId;
                    messageContentDto.ReceiverId = toUserId;

                    var messageContent = ObjectMapper.Map<CreateUpdateMessageContentDto, MessageContent>(messageContentDto);
                    var insertedMessageContent = await _messageContentRepository.InsertAsync(messageContent, true);

                    return new SuccessDataResult<string>(_L["SuccessfullyCompleted"].Value);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "MessageAppService > InsertMessageContentAsync has error! ");

                return new ErrorDataResult<string>(_L["GeneralError"].Value);
            }
        }

    }
}
