using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _365EJSC.ERP.Persistence.Configurations.Define
{
    /// <summary>
    /// EF core configuration for <see cref="WebLocalProvince"/>
    /// </summary>
    public class WebLocalProvinceConfig : IEntityTypeConfiguration<WebLocalProvince>
    {
        public void Configure(EntityTypeBuilder<WebLocalProvince> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(WebLocalProvinceConst.FIELD_ID);
            builder.Property(x => x.Name).HasColumnName(WebLocalProvinceConst.FIELD_NAME).HasMaxLength(WebLocalProvinceConst.NAME_MAX_LENGTH);
            builder.Property(x => x.NameEn).HasColumnName(WebLocalProvinceConst.FIELD_NAME_EN).HasMaxLength(WebLocalProvinceConst.NAME_EN_MAX_LENGTH);
            builder.Property(x => x.FullName).HasColumnName(WebLocalProvinceConst.FIELD_FULLNAME).HasMaxLength(WebLocalProvinceConst.FULLNAME_MAX_LENGTH);
            builder.Property(x => x.FullNameEn).HasColumnName(WebLocalProvinceConst.FIELD_FULLNAME_EN).HasMaxLength(WebLocalProvinceConst.FULLNAME_EN_MAX_LENGTH);
            builder.Property(x => x.Latitude).HasColumnName(WebLocalProvinceConst.FIELD_LATITUDE);
            builder.Property(x => x.Longitude).HasColumnName(WebLocalProvinceConst.FIELD_LONGITUDE);
            builder.Property(x => x.KeyLocalization).HasColumnName(WebLocalProvinceConst.FIELD_KEY_LOCALIZATION).HasMaxLength(WebLocalProvinceConst.KEY_LOCALIZATION_MAX_LENGTH);

            builder.HasOne(x => x.WebLocal).WithMany().HasForeignKey(x => x.KeyLocalization);

            builder.ToTable(WebLocalProvinceConst.TABLE_NAME);
        }
    }
}
