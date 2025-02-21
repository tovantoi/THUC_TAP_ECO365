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
            builder.Property(x => x.Id).HasColumnName(WebLocalWardConst.FIELD_WARD_ID);
            builder.Property(x => x.Name)
            .HasColumnName(WebLocalWardConst.FIELD_NAME)
            .HasMaxLength(WebLocalWardConst.MAX_LENGTH_NAME);

            builder.Property(x => x.NameEn)
                .HasColumnName(WebLocalWardConst.FIELD_NAME_EN)
                .HasMaxLength(WebLocalWardConst.MAX_LENGTH_NAME_EN);

            builder.Property(x => x.FullName)
                .HasColumnName(WebLocalWardConst.FIELD_FULL_NAME)
                .HasMaxLength(WebLocalWardConst.MAX_LENGTH_FULL_NAME);

            builder.Property(x => x.FullNameEn)
                .HasColumnName(WebLocalWardConst.FIELD_FULL_NAME_EN)
                .HasMaxLength(WebLocalWardConst.MAX_LENGTH_FULL_NAME_EN);

            builder.Property(x => x.Latitude)
                .HasColumnName(WebLocalWardConst.FIELD_LATITUDE);

            builder.Property(x => x.Longitude)
                .HasColumnName(WebLocalWardConst.FIELD_LONGITUDE);

            builder.Property(x => x.DistrictId)
                .HasColumnName(WebLocalWardConst.FIELD_DISTRICT_ID);

            builder.ToTable(WebLocalWardConst.TABLE_NAME);
            builder.HasOne(x => x.WebLocalDistrict)
              .WithMany()
              .HasForeignKey(x => x.DistrictId);
        }
    }
}
