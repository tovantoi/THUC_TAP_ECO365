using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Constants.HRM;
using _365EJSC.ERP.Domain.Entities.Define;
using _365EJSC.ERP.Domain.Entities.HRM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _365EJSC.ERP.Persistence.Configurations.HRM
{
	public class DegreeConfig : IEntityTypeConfiguration<Degree>
	{
		public void Configure(EntityTypeBuilder<Degree> builder)
		{
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Id).HasColumnName(DegreeConst.FIELD_DEGREE_ID);
			builder.Property(x => x.Name).HasColumnName(DegreeConst.FIELD_DEG_NAME).HasMaxLength(DegreeConst.DEG_NAME_MAX_LENGTH);
			
			builder.ToTable(DegreeConst.TABLE_NAME);
		}

	}
}