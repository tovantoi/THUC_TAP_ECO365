using _365Architect.Demo.Domain.Constants;
using _365Architect.Demo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _365Architect.Demo.Persistence.Configurations
{
    /// <summary>
    /// EF core configuration for <see cref="Sample"/>
    /// </summary>
    public class SampleConfig : IEntityTypeConfiguration<Sample>
    {
        public void Configure(EntityTypeBuilder<Sample> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Description).HasColumnName(SampleConst.FIELD_DESCRIPTION).HasMaxLength(SampleConst.DESCRIPTION_MAX_LENGTH);
            builder.Property(x => x.Title).HasColumnName(SampleConst.FIELD_TITLE).HasMaxLength(SampleConst.TITLE_MAX_LENGTH);
            builder.Property(x => x.DueDate).HasColumnName(SampleConst.FIELD_DUE_DATE);

            builder.ToTable(SampleConst.TABLE_NAME);
        }
    }
}