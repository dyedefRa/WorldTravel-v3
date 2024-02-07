namespace WorldTravel.Permissions
{
    public static class WorldTravelPermissions
    {
        public const string GroupName = "WorldTravel";
        public const string Identity = "AbpIdentity";

        public static class User
        {
            public const string Default = Identity + ".Users";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string ManagePermission = Default + ".ManagePermissions";
        }

        public static class Role
        {
            public const string Default = Identity + ".Roles";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string ManagePermission = Default + ".ManagePermissions";
        }

        public static class Form
        {
            public const string Default = Identity + ".Forms";
            public const string Edit = Default + ".Edit";
        }

        public static class CountryContent
        {
            public const string Default = Identity + ".CountryContents";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
        }

        public static class Receipt
        {
            public const string Default = Identity + ".Receipt";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
        }

        public static class VisaType
        {
            public const string Default = Identity + ".VisaTypes";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
        }

        public static class Job
        {
            public const string Default = Identity + ".Jobs";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
        }
        public static class Slider
        {
            public const string Default = Identity + ".Sliders";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
        }

        public static class Blog
        {
            public const string Default = Identity + ".Blogs";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
        }

        public static class Test
        {
            public const string Default = Identity + ".Test";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
        }

    }
}