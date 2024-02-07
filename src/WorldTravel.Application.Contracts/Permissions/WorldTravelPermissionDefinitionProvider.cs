using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using WorldTravel.Localization;

namespace WorldTravel.Permissions
{
    public class WorldTravelPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(WorldTravelPermissions.GroupName);
            //Define your own permissions here. Example:
            //myGroup.AddPermission(EvArkadasimPermissions.MyPermission1, L("Permission:MyPermission1"));

            var formPermission = myGroup.AddPermission(WorldTravelPermissions.Form.Default, L("FormManagement"));
            //formPermission.AddChild(WorldTravelPermissions.Form.Create, L("Permission:Create"));
            formPermission.AddChild(WorldTravelPermissions.Form.Edit, L("Permission:Edit"));
            //formPermission.AddChild(WorldTravelPermissions.Form.Delete, L("Permission:Delete"));
            //formPermission.AddChild(WorldTravelPermissions.Form.ManagePermissions, L("Permission:ManagePermissions"));

            var counrtyContentPermission = myGroup.AddPermission(WorldTravelPermissions.CountryContent.Default, L("CountryContentManagement"));
            counrtyContentPermission.AddChild(WorldTravelPermissions.CountryContent.Create, L("Permission:Create"));
            counrtyContentPermission.AddChild(WorldTravelPermissions.CountryContent.Edit, L("Permission:Edit"));

            var visaTypePermission = myGroup.AddPermission(WorldTravelPermissions.VisaType.Default, L("VisaTypeManagement"));
            visaTypePermission.AddChild(WorldTravelPermissions.VisaType.Create, L("Permission:Create"));
            visaTypePermission.AddChild(WorldTravelPermissions.VisaType.Edit, L("Permission:Edit"));

            var jobPermission = myGroup.AddPermission(WorldTravelPermissions.Job.Default, L("JobManagement"));
            jobPermission.AddChild(WorldTravelPermissions.Job.Create, L("Permission:Create"));
            jobPermission.AddChild(WorldTravelPermissions.Job.Edit, L("Permission:Edit"));


            var sliderPermission = myGroup.AddPermission(WorldTravelPermissions.Slider.Default, L("SliderManagament"));
            sliderPermission.AddChild(WorldTravelPermissions.Slider.Create, L("Permission:Create"));
            sliderPermission.AddChild(WorldTravelPermissions.Slider.Edit, L("Permission:Edit"));

            var receiptPermission = myGroup.AddPermission(WorldTravelPermissions.Receipt.Default, L("ReceiptPermission"));
            receiptPermission.AddChild(WorldTravelPermissions.Receipt.Create, L("Permission:Create"));
            receiptPermission.AddChild(WorldTravelPermissions.Receipt.Edit, L("Permission:Edit"));

            var blogPermission = myGroup.AddPermission(WorldTravelPermissions.Blog.Default, L("BlogPermission"));
            blogPermission.AddChild(WorldTravelPermissions.Blog.Create, L("Permission:Create"));
            blogPermission.AddChild(WorldTravelPermissions.Blog.Edit, L("Permission:Edit"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<WorldTravelResource>(name);
        }
    }
}
