using _365EJSC.ERP.Domain.Constants;
using _365EJSC.ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _365EJSC.ERP.Persistence.Configurations
{
    public class WebLocalConfig : IEntityTypeConfiguration<WebLocals>
	{
		public void Configure(EntityTypeBuilder<WebLocals> builder)
		{
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Id).HasColumnName(WebLocalConst.FIELD_KEY_LOCALIZATION).HasMaxLength(WebLocalConst.KEY_LOCALIZATION_MAX_LENGTH);
			builder.Property(x => x.Localization).HasColumnName(WebLocalConst.FIELD_LOCALIZATION).HasMaxLength(WebLocalConst.LOCALIZATION_MAX_LENGTH);
			builder.Property(x => x.IsActived).HasColumnName(WebLocalConst.FIELD_IS_ACTIVED);

			builder.ToTable(WebLocalConst.TABLE_NAME);
		}
	}
}
