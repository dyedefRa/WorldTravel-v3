using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using WorldTravel.Entities.Blogs;
using WorldTravel.Entities.Categories;
using WorldTravel.Entities.Cities;
using WorldTravel.Entities.Countries;
using WorldTravel.Entities.CountryContentFiles;
using WorldTravel.Entities.CountryContents;
using WorldTravel.Entities.Files;
using WorldTravel.Entities.Forms;
using WorldTravel.Entities.Jobs;
using WorldTravel.Entities.Logs;
using WorldTravel.Entities.MailTemplates;
using WorldTravel.Entities.MessageContents;
using WorldTravel.Entities.Messages;
using WorldTravel.Entities.Receipts;
using WorldTravel.Entities.SentMails;
using WorldTravel.Entities.Sliders;
using WorldTravel.Entities.Towns;
using WorldTravel.Entities.VisaTypes;

namespace WorldTravel.EntityFrameworkCore
{
    public static class CustomDbContextModelCreatingExtensions
    {
        //1 Datetime nullable yapmak için direk enitty de nullable ver yani ; 
        //        public DateTime? BirthDate { get; set; }

        //String required için  burdan ver entity.Property(e => e.Title).IsRequired(true) yoksa nullable oluyor.
        //İlişkileri buradan yap.
        public static void CustomConfigure(this ModelBuilder modelBuilder)
        {
            Check.NotNull(modelBuilder, nameof(modelBuilder));

            var dbTablePrefix = WorldTravelConsts.DbTablePrefix;

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable(dbTablePrefix + "Blogs");

                entity.Property(e => e.Title).IsRequired(true).HasMaxLength(255);
                entity.Property(e => e.SeoTitle).IsRequired(true);
                entity.Property(e => e.Description).IsRequired(true);
                entity.Property(e => e.SeoDescription);
                entity.Property(e => e.ReadCount).HasColumnType("int");
                entity.Property(e => e.Rank).HasColumnType("int");
                entity.Property(e => e.IsSeenHomePage).HasColumnType("bit");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.ValidDate).HasColumnType("datetime");
                entity.Property(a => a.Status).HasColumnType("int");

                entity.HasIndex(e => e.ImageId, "IX_AppBlogs_FileId");


                entity.HasOne(d => d.Image)
                 .WithMany(p => p.Blogs)
                 .HasForeignKey(d => d.ImageId)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("FK_AppBlogs_AppFiles");

            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable(dbTablePrefix + "Categories");

                entity.Property(e => e.Title).IsRequired(true).HasMaxLength(255);
                entity.Property(e => e.SeoTitle).IsRequired(true);
                entity.Property(e => e.Description).IsRequired(true);
                entity.Property(e => e.SeoDescription);
                entity.Property(e => e.IsSeenHomePage).HasColumnType("bit");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.ValidDate).HasColumnType("datetime");
                entity.Property(a => a.Status).HasColumnType("int");

            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable(dbTablePrefix + "Cities");

                entity.Property(e => e.Title).IsRequired(true).HasMaxLength(255);
                entity.Property(e => e.CityNo).HasColumnType("int");
                entity.Property(e => e.AreaNumber).IsRequired(true).HasMaxLength(255);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable(dbTablePrefix + "Countries");

                entity.Property(e => e.Title).IsRequired(true).HasMaxLength(255);
            });

            modelBuilder.Entity<CountryContentFile>(entity =>
            {
                entity.ToTable(dbTablePrefix + "CountryContentFiles");

                entity.Property(e => e.CountryContentId).HasColumnType("int");
                entity.Property(e => e.FileId).HasColumnType("int");
                entity.Property(e => e.IsShareContent).HasColumnType("bit");

                entity.HasIndex(e => e.CountryContentId, "IX_AppCountryContentFile_CountryContentId");
                entity.HasIndex(e => e.FileId, "IX_AppCountryContentFile_FileId");

                entity.HasOne(d => d.CountryContent)
                 .WithMany(p => p.CountryContentFiles)
                 .HasForeignKey(d => d.CountryContentId)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("FK_AppCountryContentFiles_AppCountryContents");

                entity.HasOne(d => d.File)
                 .WithMany(p => p.CountryContentFiles)
                 .HasForeignKey(d => d.FileId)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("FK_AppCountryContentFiles_AppFiles");
            });

            modelBuilder.Entity<CountryContent>(entity =>
            {
                entity.ToTable(dbTablePrefix + "CountryContents");

                entity.Property(e => e.Title).IsRequired(true).HasMaxLength(255);
                entity.Property(e => e.ShortDescription).IsRequired(true);
                entity.Property(e => e.Description).IsRequired(true);
                entity.Property(e => e.ExtraDescription);
                entity.Property(e => e.CountryId).HasColumnType("int");
                entity.Property(e => e.ReadCount).HasColumnType("int");
                entity.Property(e => e.Rank).HasColumnType("int");
                entity.Property(e => e.IsSeenHomePage).HasColumnType("bit");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.ValidDate).HasColumnType("datetime");
                entity.Property(a => a.Status).HasColumnType("int");

                entity.HasIndex(e => e.ImageId, "IX_AppCountryContent_FileId");
                entity.HasIndex(e => e.CountryId, "IX_AppCountryContent_CountryId");

                entity.HasOne(d => d.Image)
                 .WithMany(p => p.CountryContents)
                 .HasForeignKey(d => d.ImageId)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("FK_AppCountryContents_AppFiles");

                entity.HasOne(d => d.Country)
                 .WithMany(p => p.CountryContents)
                 .HasForeignKey(d => d.CountryId)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("FK_AppCountryContents_AppCountries");
            });

            modelBuilder.Entity<File>(entity =>
            {
                entity.ToTable(dbTablePrefix + "Files");

                entity.Property(e => e.Name).IsRequired(true).HasMaxLength(500);
                entity.Property(e => e.Path).IsRequired(true).HasMaxLength(500);
                entity.Property(e => e.FullPath).IsRequired(true).HasMaxLength(500);
                entity.Property(a => a.FileType).HasColumnType("int");
                entity.Property(a => a.Rank).HasColumnType("int");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(a => a.Status).HasColumnType("int");
            });

            modelBuilder.Entity<Form>(entity =>
            {
                entity.ToTable(dbTablePrefix + "Forms");

                entity.Property(e => e.Name).IsRequired(true).HasMaxLength(100);
                entity.Property(e => e.Surname).IsRequired(true).HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired(true).HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).IsRequired(true).HasMaxLength(100);
                entity.Property(e => e.Description).IsRequired(false);
                entity.Property(e => e.JobTitle).IsRequired(true).HasMaxLength(100);
                entity.Property(e => e.IsBecomeConsultDesc).IsRequired(true).HasMaxLength(100);
                entity.Property(e => e.Profession).IsRequired(true).HasMaxLength(100);
                entity.Property(e => e.IsNeedConsult).IsRequired(true).HasMaxLength(100);
                entity.Property(e => e.EducationStatus).IsRequired(false);
                entity.Property(e => e.CompanyName).IsRequired(true).HasMaxLength(100);
                entity.Property(e => e.CompanyContact).IsRequired(true).HasMaxLength(100);
                entity.Property(e => e.PositionName).IsRequired(true).HasMaxLength(100);
                entity.Property(e => e.SpecialRequest).IsRequired(true).HasMaxLength(100);
                entity.Property(e => e.JobTip).IsRequired(false);
                entity.Property(e => e.RequiredQualification).IsRequired(true).HasMaxLength(100);
                entity.Property(e => e.SalaryRange).IsRequired(true).HasMaxLength(100);
                entity.Property(e => e.HoursWork).IsRequired(true).HasMaxLength(100);
                entity.Property(e => e.Location).IsRequired(true).HasMaxLength(100);
                entity.Property(a => a.Gender).HasColumnType("int");
                entity.Property(e => e.BirthDate).HasColumnType("datetime");
                entity.Property(e => e.CountryId).HasColumnType("int");
                entity.Property(e => e.UserId).HasColumnType("uniqueidentifier");
                entity.Property(e => e.IsContacted).HasColumnType("bit");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.FormRegType);
                entity.Property(a => a.Status).HasColumnType("int");
                entity.Property(e => e.FormIsOk).HasColumnType("int");
                entity.HasIndex(e => e.UserId, "IX_AppCountryContent_UserId");
                entity.HasIndex(e => e.CountryId, "IX_AppForm_CountryId");

                entity.HasOne(d => d.Country)
               .WithMany(p => p.Forms)
               .HasForeignKey(d => d.CountryId)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_AppForms_AppCountries");
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable(dbTablePrefix + "Jobs");

                entity.Property(e => e.Title).IsRequired(true).HasMaxLength(255);
                entity.Property(e => e.ShortDescription).IsRequired(true);
                entity.Property(e => e.Description).IsRequired(true);
                entity.Property(e => e.ExtraDescription);
                entity.Property(e => e.HourlyRate);
                entity.Property(e => e.AccommodationFee);
                entity.Property(e => e.LanguageName);
                entity.Property(e => e.CountryId).HasColumnType("int");
                entity.Property(e => e.ReadCount).HasColumnType("int");
                entity.Property(e => e.Rank).HasColumnType("int");
                entity.Property(e => e.IsSeenHomePage).HasColumnType("bit");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.ValidDate).HasColumnType("datetime");
                entity.Property(a => a.Status).HasColumnType("int");

                entity.HasIndex(e => e.ImageId, "IX_AppJob_FileId");
                entity.HasIndex(e => e.CountryId, "IX_AppJob_CountryId");

                entity.HasOne(d => d.Image)
                 .WithMany(p => p.Jobs)
                 .HasForeignKey(d => d.ImageId)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("FK_AppJobs_AppFiles");

                entity.HasOne(d => d.Country)
                 .WithMany(p => p.Jobs)
                 .HasForeignKey(d => d.CountryId)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("FK_AppJobs_AppCountries");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable(dbTablePrefix + "Logs");

                entity.Property(e => e.Message);
                entity.Property(e => e.MessageTemplate);
                entity.Property(e => e.Level).HasMaxLength(500);
                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
                entity.Property(e => e.Exception);
                entity.Property(e => e.Properties);
            });

            modelBuilder.Entity<MailTemplate>(entity =>
            {
                entity.ToTable(dbTablePrefix + "MailTemplates");

                entity.Property(e => e.Subject).IsRequired(true).HasMaxLength(500);
                entity.Property(e => e.MailKey).IsRequired(true).HasMaxLength(100);
                entity.Property(e => e.MailTemplateValue).IsRequired(true);
                entity.Property(e => e.InsertedDate).HasColumnType("datetime");
                entity.Property(a => a.Status).HasColumnType("int");
            });

            modelBuilder.Entity<MessageContent>(entity =>
            {
                entity.ToTable(dbTablePrefix + "MessageContents");

                entity.HasIndex(e => e.MessageId, "IX_AppMessageContent_MessageId");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.MessageContents)
                    .HasForeignKey(d => d.MessageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppMessageContents_AppMessages");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable(dbTablePrefix + "Messages");

                entity.HasIndex(e => e.SenderId, "IX_AppMessage_SenderId");
                entity.HasIndex(e => e.ReceiverId, "IX_AppMessage_ReceiverId");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.SendedMessages)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppMessageContents_AppSendedMessages");
                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.ReceivedMessages)
                    .HasForeignKey(d => d.ReceiverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppMessageContents_AppReceivedMessages");
            });

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.ToTable(dbTablePrefix + "Receipts");

                entity.Property(e => e.UserId).HasColumnType("uniqueidentifier");
                entity.Property(e => e.FileId).HasColumnType("int");
                entity.Property(e => e.ReceiptOk).HasColumnType("bit");

                entity.HasIndex(e => e.UserId, "IX_AppReceipt_UserId");
                entity.HasIndex(e => e.FileId, "IX_AppReceipt_FileId");

                entity.HasOne(d => d.User)
                 .WithMany(p => p.ReceiptList)
                 .HasForeignKey(d => d.UserId)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("FK_AppReceipt_Users");

                entity.HasOne(d => d.File)
                 .WithMany(p => p.ReceiptList)
                 .HasForeignKey(d => d.FileId)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("FK_AppReceipt_AppFiles");
            });

            modelBuilder.Entity<SentMail>(entity =>
            {
                entity.ToTable(dbTablePrefix + "SentMails");

                entity.Property(e => e.ToAddress).IsRequired(false).HasMaxLength(500);
                entity.Property(e => e.CcAddress).IsRequired(false).HasMaxLength(500);
                entity.Property(e => e.BccAddress).IsRequired(false).HasMaxLength(500);
                entity.Property(e => e.Subject).IsRequired(true);
                entity.Property(e => e.Body).IsRequired(true);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Slider>(entity =>
            {
                entity.ToTable(dbTablePrefix + "Sliders");

                entity.Property(e => e.Title).IsRequired(true).HasMaxLength(255);
                entity.Property(e => e.Description).IsRequired(true);
                entity.Property(e => e.ImageId).HasColumnType("int");
                entity.Property(e => e.IsSeenHomePage).HasColumnType("bit");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.ValidDate).HasColumnType("datetime");
                entity.Property(a => a.Status).HasColumnType("int");

                entity.HasIndex(e => e.ImageId, "IX_AppSlider_FileId");

                entity.HasOne(d => d.Image)
                 .WithMany(p => p.Slider)
                 .HasForeignKey(d => d.ImageId)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("FK_AppSlider_AppFiles");

            });

            modelBuilder.Entity<Town>(entity =>
            {
                entity.ToTable(WorldTravelConsts.DbTablePrefix + "Towns");

                entity.Property(e => e.Title).IsRequired(true).HasMaxLength(255);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Towns)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppTowns_AppCities");
            });

            modelBuilder.Entity<VisaType>(entity =>
            {
                entity.ToTable(dbTablePrefix + "VisaTypes");

                entity.Property(e => e.Title).IsRequired(true).HasMaxLength(255);
                entity.Property(e => e.ShortDescription).IsRequired(true);
                entity.Property(e => e.Description).IsRequired(true);
                entity.Property(e => e.ExtraDescription);
                entity.Property(e => e.CountryId).HasColumnType("int");
                entity.Property(e => e.ReadCount).HasColumnType("int");
                entity.Property(e => e.Rank).HasColumnType("int");
                entity.Property(e => e.IsSeenHomePage).HasColumnType("bit");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.ValidDate).HasColumnType("datetime");
                entity.Property(a => a.Status).HasColumnType("int");

                entity.HasIndex(e => e.ImageId, "IX_AppVisaType_FileId");
                entity.HasIndex(e => e.CountryId, "IX_AppVisaType_CountryId");

                entity.HasOne(d => d.Image)
                 .WithMany(p => p.VisaTypes)
                 .HasForeignKey(d => d.ImageId)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("FK_AppVisaTypes_AppFiles");

                entity.HasOne(d => d.Country)
                 .WithMany(p => p.VisaTypes)
                 .HasForeignKey(d => d.CountryId)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("FK_AppVisaTypes_AppCountries");
            });

        }
    }
}
