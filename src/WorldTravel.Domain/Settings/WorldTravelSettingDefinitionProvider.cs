using Volo.Abp.Identity.Settings;
using Volo.Abp.Settings;

namespace WorldTravel.Settings
{
    public class WorldTravelSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(WorldTravelSettings.MySetting1));

            //IDENTITY OPTIONS OVERRIDE 2-2
            context.Add(new SettingDefinition(IdentitySettingNames.Password.RequiredLength, WorldTravelSettings.IdentityOptions.RequiredLength.ToString()));
            context.Add(new SettingDefinition(IdentitySettingNames.Password.RequireNonAlphanumeric, WorldTravelSettings.IdentityOptions.RequireNonAlphanumeric.ToString()));
            context.Add(new SettingDefinition(IdentitySettingNames.Password.RequireLowercase, WorldTravelSettings.IdentityOptions.RequireLowercase.ToString()));
            context.Add(new SettingDefinition(IdentitySettingNames.Password.RequireUppercase, WorldTravelSettings.IdentityOptions.RequireUppercase.ToString()));
            context.Add(new SettingDefinition(IdentitySettingNames.Password.RequireDigit, WorldTravelSettings.IdentityOptions.RequireDigit.ToString()));
            context.Add(new SettingDefinition(IdentitySettingNames.Password.RequiredUniqueChars, WorldTravelSettings.IdentityOptions.RequiredUniqueChars.ToString()));
        }
    }
}
