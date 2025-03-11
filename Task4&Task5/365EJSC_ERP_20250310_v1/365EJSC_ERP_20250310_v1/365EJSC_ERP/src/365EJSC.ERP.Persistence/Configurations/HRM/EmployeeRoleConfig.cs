using _365EJSC.ERP.Domain.Constants.HRM;
using _365EJSC.ERP.Domain.Entities.HRM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _365EJSC.ERP.Persistence.Configurations.HRM
{
    /// <summary>
    /// EF Core configuration for <see cref="HrmEmployeeRole"/>
    /// </summary>
    public class EmployeeRoleConfig : IEntityTypeConfiguration<HrmEmployeeRole>
    {
        public void Configure(EntityTypeBuilder<HrmEmployeeRole> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(EmployeeRoleConst.FIELD_ID);
            builder.Property(x => x.Name).HasColumnName(EmployeeRoleConst.FIELD_NAME);
            builder.Property(x => x.Code).HasColumnName(EmployeeRoleConst.FIELD_CODE);

            builder.ToTable(EmployeeRoleConst.TABLE_NAME);
        }
    }
}
