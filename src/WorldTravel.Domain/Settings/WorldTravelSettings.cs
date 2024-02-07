namespace WorldTravel.Settings
{
    public static class WorldTravelSettings
    {
        private const string Prefix = "WorldTravel";

        //Add your own setting names here. Example:
        //public const string MySetting1 = Prefix + ".MySetting1";

        //IDENTITY OPTIONS OVERRIDE 2-1
        public static class IdentityOptions
        {
            public const int RequiredLength = 6;
            public const bool RequireNonAlphanumeric = false;
            public const bool RequireUppercase = false;
            public const bool RequireLowercase = false;
            public const bool RequireDigit = false;
            public const int RequiredUniqueChars = 0;
        }
    }
}