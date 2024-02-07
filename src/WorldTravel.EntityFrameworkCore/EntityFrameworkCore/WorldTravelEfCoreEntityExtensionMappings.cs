using Microsoft.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;
using WorldTravel.Entities.Users;

namespace WorldTravel.EntityFrameworkCore
{
    public static class WorldTravelEfCoreEntityExtensionMappings
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            WorldTravelGlobalFeatureConfigurator.Configure();
            WorldTravelModuleExtensionConfigurator.Configure();

            OneTimeRunner.Run(() =>
            {
                /* You can configure extra properties for the
                 * entities defined in the modules used by your application.
                 *
                 * This class can be used to map these extra properties to table fields in the database.
                 *
                 * USE THIS CLASS ONLY TO CONFIGURE EF CORE RELATED MAPPING.
                 * USE WorldTravelModuleExtensionConfigurator CLASS (in the Domain.Shared project)
                 * FOR A HIGH LEVEL API TO DEFINE EXTRA PROPERTIES TO ENTITIES OF THE USED MODULES
                 *
                 * Example: Map a property to a table field:

                     ObjectExtensionManager.Instance
                         .MapEfCoreProperty<IdentityUser, string>(
                             "MyProperty",
                             (entityBuilder, propertyBuilder) =>
                             {
                                 propertyBuilder.HasMaxLength(128);
                             }
                         );

                 * See the documentation for more:
                 * https://docs.abp.io/en/abp/latest/Customizing-Application-Modules-Extending-Entities
                 */


                ObjectExtensionManager.Instance
                    .MapEfCoreProperty<IdentityUser, int?>(
                        nameof(AppUser.UserType),
                        (entityBuilder, propertyBuilder) =>
                        {
                            propertyBuilder.HasColumnName("UserType");
                        }
                    );

                ObjectExtensionManager.Instance
                      .MapEfCoreProperty<IdentityUser, int?>(
                          nameof(AppUser.Gender),
                          (entityBuilder, propertyBuilder) =>
                          {
                              propertyBuilder.HasColumnName("Gender");
                          }
                      );

                ObjectExtensionManager.Instance
                          .MapEfCoreProperty<IdentityUser, System.DateTime?>(
                              nameof(AppUser.BirthDate),
                              (entityBuilder, propertyBuilder) =>
                              {
                                  propertyBuilder.HasColumnType("datetime");
                                  propertyBuilder.HasColumnName("BirthDate");
                              }
                          );

                ObjectExtensionManager.Instance
                 .MapEfCoreProperty<IdentityUser, int?>(
                     nameof(AppUser.ProfileIsOk),
                     (entityBuilder, propertyBuilder) =>
                     {
                         propertyBuilder.HasColumnName("ProfileIsOk");
                     }
                 );

                ObjectExtensionManager.Instance
                         .MapEfCoreProperty<IdentityUser, int?>(
                             nameof(AppUser.Status),
                             (entityBuilder, propertyBuilder) =>
                             {
                                 propertyBuilder.HasColumnName("Status");
                             }
                         );

                ObjectExtensionManager.Instance
                        .MapEfCoreProperty<IdentityUser, int?>(
                            "ImageId",
                            (entityBuilder, propertyBuilder) =>
                            {
                                propertyBuilder.HasColumnName("ImageId");
                            }
                        );
              
            });
        }
    }
}
