using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using _365EJSC.ERP.Domain.Entities.Define;
using _365EJSC.ERP.Domain.Constants.Define;

namespace _365EJSC.ERP.Persistence.Configurations.Define
{
    /// <summary>
    /// EF Core configuration for <see cref="Department"/>
    /// </summary>
    public class DepartmentConfig : IEntityTypeConfiguration<GeneralDepartment>
    {
        public void Configure(EntityTypeBuilder<GeneralDepartment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(GeneralDepartmentConst.FIELD_ID);

            builder.Property(x => x.DeCode).HasColumnName(GeneralDepartmentConst.FIELD_DE_CODE)
                                           .HasMaxLength(GeneralDepartmentConst.DE_CODE_MAX_LENGTH);

            builder.Property(x => x.DeName).HasColumnName(GeneralDepartmentConst.FIELD_DE_NAME)
                                           .HasMaxLength(GeneralDepartmentConst.DE_NAME_MAX_LENGTH)
                                           .IsRequired();

            builder.Property(x => x.IsActived).HasColumnName(GeneralDepartmentConst.FIELD_IS_ACTIVED)
                                           .IsRequired();

            builder.ToTable(GeneralDepartmentConst.TABLE_NAME);
        }
    }
}
