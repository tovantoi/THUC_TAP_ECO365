using _365EJSC.ERP.Domain.Constants.HRM;
using _365EJSC.ERP.Domain.Entities.HRM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _365EJSC.ERP.Persistence.Configurations.HRM
{
    /// <summary>
    /// EF Core configuration for <see cref="HrmMarital"/>
    /// </summary>
    public class MaritalConfig : IEntityTypeConfiguration<HrmMarital>
    {
        public void Configure(EntityTypeBuilder<HrmMarital> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(MaritalConst.FIELD_ID);
            builder.Property(x => x.Name).HasColumnName(MaritalConst.FIELD_NAME);

            builder.ToTable(MaritalConst.TABLE_NAME);
        }
    }
}
