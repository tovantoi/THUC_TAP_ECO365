using _365EJSC.ERP.Domain.Constants.Product.ProductType;
using _365EJSC.ERP.Domain.Entities.Product.ProductType;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _365EJSC.ERP.Persistence.Configurations.Product.ProductType
{
    /// <summary>
    /// EF Core configuration for <see cref="PdProductType"/>
    /// </summary>
    public class ProductTypeConfig : IEntityTypeConfiguration<PdProductType>
    {
        public void Configure(EntityTypeBuilder<PdProductType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(ProductTypeConst.FIELD_ID);
            builder.Property(x => x.Name).HasColumnName(ProductTypeConst.FIELD_NAME);

            builder.ToTable(ProductTypeConst.TABLE_NAME);
        }
    }
}
