using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _365EJSC.ERP.Persistence.Configurations.Define
{
    /// <summary>
    /// EF core configuration for <see cref="ErpGeneralCompany"/>
    /// </summary>
    public class ErpGeneralCompanyConfig : IEntityTypeConfiguration<ErpGeneralCompany>
    {
        public void Configure(EntityTypeBuilder<ErpGeneralCompany> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(ErpGeneralCompanyConst.FIELD_ID);
            builder.Property(x => x.CompanyPid).HasColumnName(ErpGeneralCompanyConst.FIELD_COMPANT_PID);
            builder.Property(x => x.TaxCode).HasColumnName(ErpGeneralCompanyConst.FIELD_TAX_CODE).HasMaxLength(ErpGeneralCompanyConst.TEL_MAX_LENGTH);
            builder.Property(x => x.Name).HasColumnName(ErpGeneralCompanyConst.FIELD_NAME).HasMaxLength(ErpGeneralCompanyConst.NAME_MAX_LENGTH);
            builder.Property(x => x.Image).HasColumnName(ErpGeneralCompanyConst.FIELD_IMAGE).HasMaxLength(ErpGeneralCompanyConst.IMAGE_MAX_LENGTH);
            builder.Property(x => x.Tel).HasColumnName(ErpGeneralCompanyConst.FIELD_TEL).HasMaxLength(ErpGeneralCompanyConst.TEL_MAX_LENGTH);
            builder.Property(x => x.Email).HasColumnName(ErpGeneralCompanyConst.FIELD_EMAIL).HasMaxLength(ErpGeneralCompanyConst.EMAIL_MAX_LENGTH);
            builder.Property(x => x.Website).HasColumnName(ErpGeneralCompanyConst.FIELD_WEBSITE).HasMaxLength(ErpGeneralCompanyConst.WEBSITE_MAX_LENGTH);
            builder.Property(x => x.Founder).HasColumnName(ErpGeneralCompanyConst.FIELD_FOUNDER).HasMaxLength(ErpGeneralCompanyConst.FOUNDER_MAX_LENGTH);
            builder.Property(x => x.Ceo).HasColumnName(ErpGeneralCompanyConst.FIELD_CEO).HasMaxLength(ErpGeneralCompanyConst.CEO_MAX_LENGTH);
            builder.Property(x => x.CeoImage).HasColumnName(ErpGeneralCompanyConst.FIELD_CEO_IMAGE).HasMaxLength(ErpGeneralCompanyConst.CEO_IMAGE_MAX_LENGTH);
            builder.Property(x => x.CeoEmail).HasColumnName(ErpGeneralCompanyConst.FIELD_CEO_EMAIL).HasMaxLength(ErpGeneralCompanyConst.CEO_EMAIL_MAX_LENGTH);
            builder.Property(x => x.CeoTel).HasColumnName(ErpGeneralCompanyConst.FIELD_CEO_TEL).HasMaxLength(ErpGeneralCompanyConst.CEO_TEL_MAX_LENGTH);
            builder.Property(x => x.License).HasColumnName(ErpGeneralCompanyConst.FIELD_LICENSE).HasMaxLength(ErpGeneralCompanyConst.LICENSE_MAX_LENGTH);
            builder.Property(x => x.CountryId).HasColumnName(ErpGeneralCompanyConst.FIELD_COUNTRY_ID);
            builder.Property(x => x.WardId).HasColumnName(ErpGeneralCompanyConst.FIELD_WARD_ID);
            builder.Property(x => x.IsActived).HasColumnName(ErpGeneralCompanyConst.FIELD_IS_ACTIVED);

            builder.ToTable(ErpGeneralCompanyConst.TABLE_NAME);
        }
    }

}