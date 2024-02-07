using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.Users.EntityFrameworkCore;
using WorldTravel.Entities.Blogs;
using WorldTravel.Entities.Cities;
using WorldTravel.Entities.Countries;
using WorldTravel.Entities.CountryContentFiles;
using WorldTravel.Entities.CountryContents;
using WorldTravel.Entities.Files;
using WorldTravel.Entities.Forms;
using WorldTravel.Entities.Jobs;
using WorldTravel.Entities.Logs;
using WorldTravel.Entities.MailTemplates;
using WorldTravel.Entities.Receipts;
using WorldTravel.Entities.SentMails;
using WorldTravel.Entities.Sliders;
using WorldTravel.Entities.Towns;
using WorldTravel.Entities.Users;
using WorldTravel.Entities.VisaTypes;

namespace WorldTravel.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class WorldTravelDbContext :
        AbpDbContext<WorldTravelDbContext>
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Blog> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CountryContentFile> CountryContentFiles { get; set; }
        public DbSet<CountryContent> CountryContents { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<MailTemplate> MailTemplates { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<SentMail> SentMails { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<VisaType> VisaTypes { get; set; }
        public WorldTravelDbContext(DbContextOptions<WorldTravelDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurePermissionManagement();
            builder.ConfigureSettingManagement();
            builder.ConfigureBackgroundJobs();
            builder.ConfigureAuditLogging();
            builder.ConfigureIdentity();
            builder.ConfigureIdentityServer();
            builder.ConfigureFeatureManagement();
            builder.ConfigureTenantManagement();

            //TODOO 3
            builder.Entity<AppUser>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "Users");
                b.ConfigureByConvention();
                b.ConfigureAbpUser();
                b.Property(a => a.UserType).HasColumnName("UserType").HasColumnType("int");
                b.Property(a => a.Gender).HasColumnName("Gender").HasColumnType("int");
                b.Property(a => a.BirthDate).HasColumnName("BirthDate").HasColumnType("datetime");
                b.Property(a => a.ProfileIsOk).HasColumnName("ProfileIsOk").HasColumnType("int");
                b.Property(a => a.ImageId).HasColumnName("ImageId").HasColumnType("int");
                b.Property(a => a.Status).HasColumnName("Status").HasColumnType("int");
                b.HasOne<IdentityUser>().WithOne().HasForeignKey<AppUser>(x => x.Id);
            });

            builder.CustomConfigure();
        }
    }
}
