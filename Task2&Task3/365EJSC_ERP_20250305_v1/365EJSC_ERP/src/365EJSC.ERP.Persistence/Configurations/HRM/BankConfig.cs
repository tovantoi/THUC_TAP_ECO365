using _365EJSC.ERP.Domain.Constants.HRM;
using _365EJSC.ERP.Domain.Entities.HRM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _365EJSC.ERP.Persistence.Configurations.HRM
{
    /// <summary>
    /// EF Core configuration for <see cref="HrmBank"/>
    /// </summary>
    public class BankConfig : IEntityTypeConfiguration<HrmBank>
    {
        public void Configure(EntityTypeBuilder<HrmBank> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(BankConst.FIELD_ID);
            builder.Property(x => x.Name).HasColumnName(BankConst.FIELD_NAME);

            builder.ToTable(BankConst.TABLE_NAME);
        }
    }
}
