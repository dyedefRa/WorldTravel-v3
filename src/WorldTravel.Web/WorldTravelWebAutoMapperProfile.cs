using AutoMapper;
using Volo.Abp.Identity;
using WorldTravel.Dtos.Blog;
using WorldTravel.Dtos.Countries;
using WorldTravel.Dtos.CountryContents;
using WorldTravel.Dtos.Forms;
using WorldTravel.Dtos.Forms.ViewModels;
using WorldTravel.Dtos.Jobs;
using WorldTravel.Dtos.Receipts;
using WorldTravel.Dtos.Sliders;
using WorldTravel.Dtos.Users.ViewModels;
using WorldTravel.Dtos.VisaTypes;
using WorldTravel.Entities.Countries;
using WorldTravel.Entities.Receipts;
using static WorldTravel.Web.Pages.Account.ManageModel;
using static WorldTravel.Web.Pages.Account.RegisterModel;
using static WorldTravel.Web.Pages.Admin.Blog.CreateModel;
using static WorldTravel.Web.Pages.Admin.Blog.EditModel;
using static WorldTravel.Web.Pages.Admin.CountryContent.EditModel;
using static WorldTravel.Web.Pages.Admin.Job.CreateModel;
using static WorldTravel.Web.Pages.Admin.Job.EditModel;
using static WorldTravel.Web.Pages.Admin.Slider.CreateModel;
using static WorldTravel.Web.Pages.Admin.Slider.EditModel;
using static WorldTravel.Web.Pages.Admin.VisaType.CreateModel;
using static WorldTravel.Web.Pages.Admin.VisaType.EditModel;

namespace WorldTravel.Web
{
    public class WorldTravelWebAutoMapperProfile : Profile
    {
        public WorldTravelWebAutoMapperProfile()
        {
            //Define your AutoMapper configuration here for the Web project.

            #region User
            CreateMap<UserRegisterModel, IdentityUserCreateDto>();//Account/Register
            CreateMap<AppUserViewModel, UserManageModel>(); //Account/Manage/OnGetAsync
            #endregion

            #region Form
            CreateMap<Pages.Home.IndexModel.FormModel, CreateUpdateFormDto>();//Form/Index POST
            CreateMap<Pages.Country.DetailModel.CreateCountryContentModel, CreateUpdateFormDto>();//Country/Detail POST
            CreateMap<Pages.VisaType.DetailModel.FormModel, CreateUpdateFormDto>();//Country/Detail POST
            CreateMap<FormDto, FormViewModel>();//Country/Detail POST
            CreateMap<FormDto, CreateUpdateFormDto>();//Country/Detail POST

            #endregion

            #region Receipt
            CreateMap<Receipt, ReceiptDto>().ReverseMap();
            CreateMap<ReceiptModel, CreateUpdateReceiptDto>().ReverseMap();
            CreateMap<ReceiptDto, CreateUpdateReceiptDto>().ReverseMap();

            #endregion

            #region CountryContent
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<CountryContentDto, EditCountryContentModel>().ReverseMap();
            CreateMap<CountryContentDto, CreateUpdateCountryContentDto>().ReverseMap();
            CreateMap<Pages.Admin.CountryContent.CreateModel.CreateCountryContentModel, CreateUpdateCountryContentDto>().ReverseMap();
            #endregion

            #region VisaType
            CreateMap<CreateVisaTypeModel, CreateUpdateVisaTypeDto>().ReverseMap();
            CreateMap<VisaTypeDto, EditVisaModel>().ReverseMap();
            CreateMap<VisaTypeDto, CreateUpdateVisaTypeDto>().ReverseMap();
            #endregion

            #region Job
            CreateMap<CreateJobModel, CreateUpdateJobDto>().ReverseMap();
            CreateMap<JobDto, EditJobModel>().ReverseMap();
            CreateMap<JobDto, CreateUpdateJobDto>().ReverseMap();
            #endregion

            #region Blog
            CreateMap<CreateBlogModel, CreateUpdateBlogDto>().ReverseMap();
            CreateMap<BlogDto, EditBlogModel>().ReverseMap();
            CreateMap<BlogDto, CreateUpdateBlogDto>().ReverseMap();
            #endregion

            #region Slider
            CreateMap<CreateSliderModel, CreateUpdateSliderDto>().ReverseMap();
            CreateMap<SliderDto, EditSliderModel>().ReverseMap();
            CreateMap<SliderDto, CreateUpdateSliderDto>().ReverseMap();

            #endregion
        }
    }
}
