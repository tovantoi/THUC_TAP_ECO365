using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _365EJSC.ERP.Persistence.Configurations.Define
{
    public class WebLocalWardConfig : IEntityTypeConfiguration<WebLocalWard>
    {
        public void Configure(EntityTypeBuilder<WebLocalWard> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(WebLocalWardConstants.FIELD_WARD_ID);
            builder.Property(x => x.Name)
            .HasColumnName(WebLocalWardConstants.FIELD_NAME)
            .HasMaxLength(WebLocalWardConstants.MAX_LENGTH_NAME);

            builder.Property(x => x.NameEn)
                .HasColumnName(WebLocalWardConstants.FIELD_NAME_EN)
                .HasMaxLength(WebLocalWardConstants.MAX_LENGTH_NAME_EN);

            builder.Property(x => x.FullName)
                .HasColumnName(WebLocalWardConstants.FIELD_FULL_NAME)
                .HasMaxLength(WebLocalWardConstants.MAX_LENGTH_FULL_NAME);

            builder.Property(x => x.FullNameEn)
                .HasColumnName(WebLocalWardConstants.FIELD_FULL_NAME_EN)
                .HasMaxLength(WebLocalWardConstants.MAX_LENGTH_FULL_NAME_EN);

            builder.Property(x => x.Latitude)
                .HasColumnName(WebLocalWardConstants.FIELD_LATITUDE);

            builder.Property(x => x.Longitude)
                .HasColumnName(WebLocalWardConstants.FIELD_LONGITUDE);

            builder.Property(x => x.DistrictId)
                .HasColumnName(WebLocalWardConstants.FIELD_DISTRICT_ID);

            builder.ToTable(WebLocalWardConstants.TABLE_NAME);
            builder.HasOne(x => x.WebsiteLocalizationDictrict)
              .WithMany()
              .HasForeignKey(x => x.DistrictId);
        }
    }
}
