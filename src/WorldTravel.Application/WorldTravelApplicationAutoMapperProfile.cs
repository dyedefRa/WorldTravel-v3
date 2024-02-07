using AutoMapper;
using Volo.Abp.Identity;
using WorldTravel.Dtos.Blog;
using WorldTravel.Dtos.Blog.ViewModels;
using WorldTravel.Dtos.CountryContents;
using WorldTravel.Dtos.CountryContents.ViewModels;
using WorldTravel.Dtos.Files;
using WorldTravel.Dtos.Files.ViewModels;
using WorldTravel.Dtos.Forms;
using WorldTravel.Dtos.Forms.ViewModels;
using WorldTravel.Dtos.Jobs;
using WorldTravel.Dtos.Jobs.ViewModels;
using WorldTravel.Dtos.MailTemplates;
using WorldTravel.Dtos.Receipts;
using WorldTravel.Dtos.SentMails;
using WorldTravel.Dtos.Sliders;
using WorldTravel.Dtos.Sliders.ViewModels;
using WorldTravel.Dtos.Users;
using WorldTravel.Dtos.Users.ViewModels;
using WorldTravel.Dtos.VisaTypes;
using WorldTravel.Dtos.VisaTypes.ViewModels;
using WorldTravel.Entities.Blogs;
using WorldTravel.Entities.CountryContents;
using WorldTravel.Entities.Files;
using WorldTravel.Entities.Forms;
using WorldTravel.Entities.Jobs;
using WorldTravel.Entities.MailTemplates;
using WorldTravel.Entities.Receipts;
using WorldTravel.Entities.SentMails;
using WorldTravel.Entities.Sliders;
using WorldTravel.Entities.Users;
using WorldTravel.Entities.VisaTypes;
using WorldTravel.Models.Pages.Account;

namespace WorldTravel
{
    public class WorldTravelApplicationAutoMapperProfile : Profile
    {
        public WorldTravelApplicationAutoMapperProfile()
        {
            CreateMap<MailTemplate, MailTemplateDto>();
            CreateMap<SentMailDto, SentMail>();

            #region User
            CreateMap<AppUser, AppUserViewModel>();              //UserAppService > GetUserByIdAsync
            CreateMap<IdentityUserDto, IdentityUserUpdateDto>(); //UserAppService > ManageProfileAsync
            CreateMap<AppUser, AppUserDto>().ReverseMap();
            CreateMap<Volo.Abp.Identity.IdentityUser, CustomIdentityUserDto>().ReverseMap();

            #endregion

            #region File
            CreateMap<File, FileDto>().ReverseMap();//FileAppService > SaveFileAsync
            CreateMap<File, FileViewModel>().ReverseMap();//FileAppService > SaveFileAsync

            #endregion

            #region Form
            CreateMap<Form, FormDto>().ReverseMap();//Form/Index POST
            CreateMap<Form, CreateUpdateFormDto>().ReverseMap();//Form/Index POST
            CreateMap<Form, FormViewModel>().ReverseMap();//FormAopService > GetFormListAsync
            #endregion

            #region Receipt
            CreateMap<Receipt, ReceiptDto>().ReverseMap();
            CreateMap<Receipt, ReceiptViewModel>().ReverseMap();
            CreateMap<ReceiptDto, CreateUpdateReceiptDto>().ReverseMap();

            #endregion

            #region Slider
            CreateMap<Slider, SliderDto>().ReverseMap();
            CreateMap<Slider, CreateUpdateSliderDto>().ReverseMap();
            CreateMap<Slider, SliderViewModel>().ReverseMap();
            #endregion

            #region CountryContent
            CreateMap<CountryContent, CountryContentDto>().ReverseMap();
            CreateMap<CountryContent, CreateUpdateCountryContentDto>().ReverseMap();
            CreateMap<CountryContent, CountryContentViewModel>().ReverseMap();
            #endregion

            #region VisaType
            CreateMap<VisaType, VisaTypeDto>().ReverseMap();
            CreateMap<VisaType, CreateUpdateVisaTypeDto>().ReverseMap();
            CreateMap<VisaType, VisaTypeViewModel>().ReverseMap();
            #endregion

            #region Job
            CreateMap<Job, JobDto>().ReverseMap();
            CreateMap<Job, CreateUpdateJobDto>().ReverseMap();
            CreateMap<Job, JobViewModel>().ReverseMap();
            #endregion

            #region Blog
            CreateMap<Blog, BlogDto>().ReverseMap();
            CreateMap<Blog, CreateUpdateBlogDto>().ReverseMap();
            CreateMap<Blog, BlogViewModel>().ReverseMap();
            #endregion

        }
    }
}