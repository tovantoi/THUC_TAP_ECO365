using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _365EJSC.ERP.Persistence.Configurations.Define
{
    public class ErpGeneralCompanyPositionConfig : IEntityTypeConfiguration<ErpGeneralCompanyPosition>
    {
        public void Configure(EntityTypeBuilder<ErpGeneralCompanyPosition> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(ErpGeneralCompanyPositionConst.FIELD_ID);
            builder.Property(x => x.CompanyId).HasColumnName(ErpGeneralCompanyPositionConst.FIELD_COMPANY_ID);
            builder.Property(x => x.PositionId).HasColumnName(ErpGeneralCompanyPositionConst.FIELD_POSITION_ID);

            builder.ToTable(ErpGeneralCompanyPositionConst.TABLE_NAME);
        }
    }
}