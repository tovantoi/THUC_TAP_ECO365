using _365EJSC.ERP.Domain.Constants.HRM;
using _365EJSC.ERP.Domain.Entities.HRM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _365EJSC.ERP.Persistence.Configurations.HRM
{
    public class ContractTypeConfig : IEntityTypeConfiguration<DefineContractType>
    {
        public void Configure(EntityTypeBuilder<DefineContractType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(ContractTypeConst.FIELD_ID);
            builder.Property(x => x.Code).HasColumnName(ContractTypeConst.FIELD_CODE).HasMaxLength(ContractTypeConst.CODE_MAX_LENGTH);
            builder.Property(x => x.Name).HasColumnName(ContractTypeConst.FIELD_NAME).HasMaxLength(ContractTypeConst.NAME_MAX_LENGTH);

            builder.ToTable(ContractTypeConst.TABLE_NAME);
        }
    }
}
