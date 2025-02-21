using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _365EJSC.ERP.Persistence.Configurations.Define
{
    public class WebsiteLocalizationDictrictConfig : IEntityTypeConfiguration<WebsiteLocalizationDictrict>
    {
        public void Configure(EntityTypeBuilder<WebsiteLocalizationDictrict> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(WebsiteLocalizationDictrictConstants.FIELD_ID_DICTRICT);
            builder.Property(x => x.Name).HasColumnName(WebsiteLocalizationDictrictConstants.FIELD_NAME);
            builder.ToTable(WebsiteLocalizationDictrictConstants.TABLE_NAME);
        }
    }
}
