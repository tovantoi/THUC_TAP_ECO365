using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _365EJSC.ERP.Persistence.Configurations.Define
{
    /// <summary>
    /// EF core configuration for <see cref="ErpGeneralPosition"/>
    /// </summary>
    public class ErpGeneralPositionConfig : IEntityTypeConfiguration<ErpGeneralPosition>
    {
        public void Configure(EntityTypeBuilder<ErpGeneralPosition> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(ErpGeneralPositionConst.FIELD_ID);
            builder.Property(x => x.Code).HasColumnName(ErpGeneralPositionConst.FIELD_CODE).HasMaxLength(ErpGeneralPositionConst.CODE_MAX_LENGTH);
            builder.Property(x => x.Name).HasColumnName(ErpGeneralPositionConst.FIELD_NAME).HasMaxLength(ErpGeneralPositionConst.NAME_MAX_LENGTH);

            builder.ToTable(ErpGeneralPositionConst.TABLE_NAME);
        }
    }
}
