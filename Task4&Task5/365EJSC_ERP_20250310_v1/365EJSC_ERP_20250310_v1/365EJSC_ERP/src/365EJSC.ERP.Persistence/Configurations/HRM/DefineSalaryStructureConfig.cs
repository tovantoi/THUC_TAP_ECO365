using _365EJSC.ERP.Domain.Constants.HRM;
using _365EJSC.ERP.Domain.Entities.HRM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _365EJSC.ERP.Persistence.Configurations.HRM
{
    public class DefineSalaryStructureConfig : IEntityTypeConfiguration<DefineSalaryStructure>
    {
        public void Configure(EntityTypeBuilder<DefineSalaryStructure> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(DefineSalaryStructureConst.FIELD_ID);
            builder.Property(x => x.Code).HasColumnName(DefineSalaryStructureConst.FIELD_CODE).HasMaxLength(DefineSalaryStructureConst.CODE_MAX_LENGTH);
            builder.Property(x => x.Name).HasColumnName(DefineSalaryStructureConst.FIELD_NAME).HasMaxLength(DefineSalaryStructureConst.NAME_MAX_LENGTH);

            builder.ToTable(DefineSalaryStructureConst.TABLE_NAME);
        }
    }
}