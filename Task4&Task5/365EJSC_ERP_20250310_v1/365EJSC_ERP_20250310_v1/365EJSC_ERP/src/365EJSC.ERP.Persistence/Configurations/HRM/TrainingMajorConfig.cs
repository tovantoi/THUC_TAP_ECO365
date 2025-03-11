using _365EJSC.ERP.Domain.Constants.HRM;
using _365EJSC.ERP.Domain.Entities.HRM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _365EJSC.ERP.Persistence.Configurations.HRM
{
    public class TrainingMajorConfig : IEntityTypeConfiguration<TrainingMajor>
    {
        public void Configure(EntityTypeBuilder<TrainingMajor> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(TrainingMajorConst.FIELD_ID);

            builder.Property(x => x.TmName)
            .HasColumnName(TrainingMajorConst.FIELD_NAME)
            .HasMaxLength(TrainingMajorConst.MAX_LENGTH_NAME);

           
            builder.ToTable(TrainingMajorConst.TABLE_NAME);
        }
    }
}
