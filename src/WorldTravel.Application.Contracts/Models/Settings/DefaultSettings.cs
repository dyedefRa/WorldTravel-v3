namespace WorldTravel.Models.Settings
{
    public class DefaultSettings
    {
        public string AutoResendExpressionMinute { get; set; }
        public int ResetPasswordTokenExpireMunite { get; set; }
        public string[] ToMailsForMailTemplates { get; set; }
        public string[] BccMailsForMailTemplates { get; set; }
        public bool UseJobs { get; set; }
    }
}
