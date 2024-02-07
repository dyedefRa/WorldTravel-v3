namespace WorldTravel.Models.Settings
{
    public class SmtpSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpServerPort { get; set; }
        public string SmtpMailAddress { get; set; }
        public string SmtpPassword { get; set; }
        public bool SmtpEnableSSL { get; set; }
        public bool SmtpUseDefaultCredentials { get; set; }
        public string SmtpDisplayName { get; set; }
    }
}
