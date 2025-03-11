using _365EJSC.ERP.Domain.Constants.Define.WebLocalDistricts;
using _365EJSC.ERP.Domain.Entities.Define;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _365EJSC.ERP.Persistence.Configurations.Define
{
    /// <summary>
    /// EF Core configuration for <see cref="WebLocalDistrict"/>
    /// </summary>
    public class WebLocalDistrictConfig : IEntityTypeConfiguration<WebLocalDistrict>
    {
        public void Configure(EntityTypeBuilder<WebLocalDistrict> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(WebLocalDistrictConst.FIELD_ID);


            builder.Property(x => x.Name).HasColumnName(WebLocalDistrictConst.FIELD_NAME).HasMaxLength(WebLocalDistrictConst.NAME_MAX_LENGTH).IsRequired();
            builder.Property(x => x.NameEn).HasColumnName(WebLocalDistrictConst.FIELD_NAME_EN).HasMaxLength(WebLocalDistrictConst.NAME_EN_MAX_LENGTH).IsRequired();
            builder.Property(x => x.FullName).HasColumnName(WebLocalDistrictConst.FIELD_FULL_NAME).HasMaxLength(WebLocalDistrictConst.FULL_NAME_MAX_LENGTH).IsRequired();
            builder.Property(x => x.FullNameEn).HasColumnName(WebLocalDistrictConst.FIELD_FULL_NAME_EN).HasMaxLength(WebLocalDistrictConst.FULL_NAME_EN_MAX_LENGTH).IsRequired();
            builder.Property(x => x.Latitude).HasColumnName(WebLocalDistrictConst.FIELD_LATITUDE).IsRequired();
            builder.Property(x => x.Longitude).HasColumnName(WebLocalDistrictConst.FIELD_LONGITUDE).IsRequired();
            builder.Property(x => x.ProvinceId).HasColumnName(WebLocalDistrictConst.FIELD_PROVINCE_ID).IsRequired();
            builder.HasOne(x => x.WebLocalProvince)
                       .WithMany()
                       .HasForeignKey(x => x.ProvinceId);


            builder.ToTable(WebLocalDistrictConst.TABLE_NAME);
        }
    }
}
