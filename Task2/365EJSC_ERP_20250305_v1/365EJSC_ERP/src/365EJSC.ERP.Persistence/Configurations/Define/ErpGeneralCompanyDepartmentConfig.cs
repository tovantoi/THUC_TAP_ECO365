using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _365EJSC.ERP.Persistence.Configurations.Define
{
    /// <summary>
    /// EF core configuration for <see cref="ErpGeneralCompanyDepartment"/>
    /// </summary>
    public class ErpGeneralCompanyDepartmentConfig : IEntityTypeConfiguration<ErpGeneralCompanyDepartment>
    {
        public void Configure(EntityTypeBuilder<ErpGeneralCompanyDepartment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(ErpGeneralCompanyDepartmentConst.FIELD_ID);
            builder.Property(x => x.CompanyId).HasColumnName(ErpGeneralCompanyDepartmentConst.FIELD_COMPANY_ID);
            builder.Property(x => x.DepartmentId).HasColumnName(ErpGeneralCompanyDepartmentConst.FIELD_DEPARTMENT_ID);

            builder.ToTable(ErpGeneralCompanyDepartmentConst.TABLE_NAME);
        }
    }
}