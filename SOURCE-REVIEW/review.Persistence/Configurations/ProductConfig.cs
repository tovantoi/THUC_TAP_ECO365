using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using review.Domain.Constants;
using review.Domain.Entities;

namespace review.Persistence.Configurations
{
    /// <summary>
    /// EF core configuration for <see cref="Product"/>
    /// </summary>
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Description).HasColumnName(ProductConst.FIELD_DESCRIPTION).HasMaxLength(ProductConst.DESCRIPTION_MAX_LENGTH);
            builder.Property(x => x.Name).HasColumnName(ProductConst.FIELD_NAME).HasMaxLength(ProductConst.NAME_MAX_LENGTH);

            builder.ToTable(ProductConst.TABLE_NAME);
        }
    }
}