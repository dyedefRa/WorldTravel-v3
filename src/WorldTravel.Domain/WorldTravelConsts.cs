namespace WorldTravel
{
    public static class WorldTravelConsts
    {
        public const string DbTablePrefix = "App";

        public const string DbSchema = null;

        #region MailTemplateKey

        public static class MailTemplateKey
        {
            public const string TemplateBody = "template_body";
            public const string TemplateFooter = "template_footer";
            public const string NewPassword = "new_password";

        }

        #endregion

        //public static string GetFullImagePath(string imagePath)
        //{
        //    return "~/web/images/" + imagePath;
        //}
        public static class DEFAULT
        {
            public static string MaleAvatarImageUrl = "/web/images/avatar-male.png";
            public static string FemaleAvatarImageUrl = "/web/images/avatar-female.png";
            public static string AdminAvatarImageUrl = "/web/images/avatar-admin.png";

            /// <summary>
            /// Filtrelerde kullanılan default tarih değeri
            /// </summary>
            //public static DateTime DefaultDate = new DateTime(2000, 01, 01);

            /// <summary>
            /// Bir Firma oluştugu andan itibaren 3 ay aktif olsun.
            /// </summary>
            //public static int DefaultCompanyValidDateAddingMonth = 3;

        }
    }
}
