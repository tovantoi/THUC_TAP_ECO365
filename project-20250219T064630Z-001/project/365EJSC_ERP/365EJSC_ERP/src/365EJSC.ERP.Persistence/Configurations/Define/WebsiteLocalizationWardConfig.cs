using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _365EJSC.ERP.Persistence.Configurations.Define
{
    public class WebsiteLocalizationWardConfig : IEntityTypeConfiguration<WebsiteLocalizationWard>
    {
        public void Configure(EntityTypeBuilder<WebsiteLocalizationWard> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(WebsiteLocalizationWardConstants.FIELD_WARD_ID);
            builder.Property(x => x.Name)
            .HasColumnName(WebsiteLocalizationWardConstants.FIELD_NAME)
            .HasMaxLength(WebsiteLocalizationWardConstants.MAX_LENGTH_NAME);

            builder.Property(x => x.NameEn)
                .HasColumnName(WebsiteLocalizationWardConstants.FIELD_NAME_EN)
                .HasMaxLength(WebsiteLocalizationWardConstants.MAX_LENGTH_NAME_EN);

            builder.Property(x => x.FullName)
                .HasColumnName(WebsiteLocalizationWardConstants.FIELD_FULL_NAME)
                .HasMaxLength(WebsiteLocalizationWardConstants.MAX_LENGTH_FULL_NAME);

            builder.Property(x => x.FullNameEn)
                .HasColumnName(WebsiteLocalizationWardConstants.FIELD_FULL_NAME_EN)
                .HasMaxLength(WebsiteLocalizationWardConstants.MAX_LENGTH_FULL_NAME_EN);

            builder.Property(x => x.Latitude)
                .HasColumnName(WebsiteLocalizationWardConstants.FIELD_LATITUDE);

            builder.Property(x => x.Longitude)
                .HasColumnName(WebsiteLocalizationWardConstants.FIELD_LONGITUDE);

            builder.Property(x => x.DistrictId)
                .HasColumnName(WebsiteLocalizationWardConstants.FIELD_DISTRICT_ID);

            builder.ToTable(WebsiteLocalizationWardConstants.TABLE_NAME);
            builder.HasOne(x => x.WebsiteLocalizationDictrict)
              .WithMany()
              .HasForeignKey(x => x.DistrictId);
        }
    }
}
