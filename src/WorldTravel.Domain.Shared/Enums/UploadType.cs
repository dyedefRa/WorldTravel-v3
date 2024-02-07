using System.ComponentModel;

namespace WorldTravel.Enums
{
    public enum UploadType
    {
        [Description("profile")]
        Profile = 0,
        [Description("countrycontent")]
        CountryContent = 1,
        [Description("sharecontent")]
        ShareContent = 2,
        [Description("visatypecontent")]
        VisaType = 3,
        [Description("job")]
        Job = 3,
        [Description("form")]
        Form = 3,
    }
}
