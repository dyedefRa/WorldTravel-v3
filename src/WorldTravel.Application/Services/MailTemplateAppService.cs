using WorldTravel.Abstract;
using WorldTravel.Dtos.MailTemplates;
using WorldTravel.Dtos.SentMails;
using WorldTravel.Entities.MailTemplates;
using WorldTravel.Models.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace WorldTravel.Services
{
    [AllowAnonymous]
    [RemoteService(IsEnabled = false, IsMetadataEnabled = false)]
    public class MailTemplateAppService : CrudAppService<MailTemplate, MailTemplateDto, int, PagedAndSortedResultRequestDto, MailTemplateDto, MailTemplateDto>, IMailTemplateAppService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ISentMailAppService _sentMailAppService;
        private readonly DefaultSettings _defaultSettings;

        public MailTemplateAppService(
            IRepository<MailTemplate, int> repository,
            IOptions<SmtpSettings> smtpSettings,
            ISentMailAppService sentMailAppService,
            IOptions<DefaultSettings> defaultSettings) : base(repository)
        {
            _smtpSettings = smtpSettings.Value;
            _sentMailAppService = sentMailAppService;
            _defaultSettings = defaultSettings.Value;
        }

        #region General Methods

        private async Task<MailTemplateDto> GetMailTemplateByMailKey(string key)
        {
            var result = await Repository.FirstOrDefaultAsync(x => x.MailKey == key);
            return ObjectMapper.Map<MailTemplate, MailTemplateDto>(result);
        }

        private async Task<string> GetMailTemplateFooter()
        {
            string footerHtml = "";
            var mailTemplate = await GetMailTemplateByMailKey(WorldTravelConsts.MailTemplateKey.TemplateFooter);
            if (mailTemplate != null)
                footerHtml = mailTemplate.MailTemplateValue;
            return footerHtml;
        }

        private async Task<MailResultModel> GetMailTemplateBody(string key)
        {
            MailResultModel mailViewModel = new MailResultModel();

            var mainTemplate = await GetMailTemplateByMailKey(WorldTravelConsts.MailTemplateKey.TemplateBody);
            if (mainTemplate != null)
            {
                var mailBody = await GetMailTemplateByMailKey(key);
                mailViewModel.Template = mainTemplate.MailTemplateValue
                    //.Replace("#Url", ConfigSettings.WebSiteUrl)
                    //.Replace("#LogoUrl", ConfigSettings.WebSiteUrl + "assets/images/logo.png")
                    .Replace("#Url", "https://www.its.gov.tr/")
                    .Replace("#LogoUrl", "https://www.its.gov.tr/Content/images/its-logo.png")
                    .Replace("#Footer", await GetMailTemplateFooter());

                if (mailBody != null)
                {
                    mailViewModel.Template = mailViewModel.Template.Replace("#Body", mailBody.MailTemplateValue);
                    mailViewModel.Subject = mailBody.Subject;
                }
                else
                {
                    mailViewModel.Status = false;
                }
            }
            else
            {
                mailViewModel.Status = false;
            }
            return mailViewModel;
        }

        private async Task<bool> SendMail(List<string> toAddress, string subject, string body, List<string> ccAddress = null, List<string> bccAddress = null, bool stopSendingMail = false, bool sendAllToMails = true, bool sendAllBccMails = true)
        {
            try
            {
                MailMessage mT = new MailMessage
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    From = new MailAddress(_smtpSettings.SmtpMailAddress, _smtpSettings.SmtpDisplayName),
                    SubjectEncoding = Encoding.GetEncoding("iso-8859-9"),
                    BodyEncoding = Encoding.GetEncoding("iso-8859-9")
                };
                if (toAddress != null)
                {
                    foreach (var mail in toAddress)
                    {
                        mT.To.Add(new MailAddress(mail));
                    }
                }
                if (ccAddress != null)
                {
                    foreach (var mail in ccAddress)
                    {
                        mT.CC.Add(new MailAddress(mail));
                    }
                }
                if (bccAddress != null)
                {
                    foreach (var mail in bccAddress)
                    {
                        mT.Bcc.Add(new MailAddress(mail));
                    }
                }
                if (sendAllToMails)
                {
                    if (_defaultSettings.ToMailsForMailTemplates != null)
                    {
                        var toMailList = _defaultSettings.ToMailsForMailTemplates?.ToList();
                        if (toMailList != null)
                        {
                            foreach (var mail in toMailList)
                            {
                                mT.To.Add(new MailAddress(mail));
                            }
                        }
                    }
                }
                if (sendAllBccMails)
                {
                    if (_defaultSettings.BccMailsForMailTemplates != null)
                    {
                        var bccMailList = _defaultSettings.BccMailsForMailTemplates?.ToList();
                        if (bccMailList != null)
                        {
                            foreach (var mail in bccMailList)
                            {
                                mT.Bcc.Add(new MailAddress(mail));
                            }
                        }
                    }
                }

                var sendedMail = new SentMailDto
                {
                    ToAddress = string.Join(";", mT.To.Select(x => x.Address)),
                    CcAddress = string.Join(";", mT.CC.Select(x => x.Address)),
                    BccAddress = string.Join(";", mT.Bcc.Select(x => x.Address)),
                    Subject = mT.Subject,
                    Body = mT.Body,
                    CreatedDate = DateTime.Now
                };

                if (stopSendingMail)
                {
                    sendedMail.Subject = "BLOKLANDI_" + sendedMail.Subject;
                }
                else
                {
                    using (SmtpClient client = new SmtpClient(_smtpSettings.SmtpServer, _smtpSettings.SmtpServerPort))
                    {
                        client.EnableSsl = _smtpSettings.SmtpEnableSSL;
                        client.UseDefaultCredentials = _smtpSettings.SmtpUseDefaultCredentials;
                        //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        if (!Convert.ToBoolean(_smtpSettings.SmtpUseDefaultCredentials))
                            client.Credentials = new NetworkCredential(_smtpSettings.SmtpMailAddress, _smtpSettings.SmtpPassword);

                        client.Send(mT);
                    }
                }

                var result = await _sentMailAppService.SaveAsync(sendedMail);
                return true;

            }
            catch (Exception ex)
            {
                Log.Error(ex, "MailTemplateAppService > SendMail has error!");
                return false;
            }
        }

        #endregion

        public async Task<bool> SendNewPasswordMailAsync(string passwordChangeUrl, string toMail)
        {
            try
            {
                var mailTemplate = await GetMailTemplateBody(WorldTravelConsts.MailTemplateKey.NewPassword);
                if (mailTemplate.Status)
                {
                    string body = mailTemplate.Template
                        .Replace("#passwordChangeUrl", passwordChangeUrl);


                    var result = await SendMail(new List<string> { toMail }, mailTemplate.Subject, body, sendAllToMails: false, sendAllBccMails: false);
                    return result;
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "MailTemplateAppService > SendNewPasswordMailAsync has error!");
                return false;
            }
        }

        #region MailResultModel

        public class MailResultModel
        {
            public string Subject { get; set; }
            public string Template { get; set; }
            public bool Status { get; set; } = true;
        }

        #endregion
    }
}
