using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _365EJSC.ERP.Persistence.Configurations.Define
{
    public class WebLocalDictrictConfig : IEntityTypeConfiguration<WebLocalDictrict>
    {
        public void Configure(EntityTypeBuilder<WebLocalDictrict> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(WebLocalDictrictConstants.FIELD_ID_DICTRICT);
            builder.Property(x => x.Name).HasColumnName(WebLocalDictrictConstants.FIELD_NAME);
            builder.ToTable(WebLocalDictrictConstants.TABLE_NAME);
        }
    }
}
