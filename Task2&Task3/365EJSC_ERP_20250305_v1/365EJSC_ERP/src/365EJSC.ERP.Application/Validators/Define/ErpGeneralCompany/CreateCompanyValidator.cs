using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.Define;

namespace _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompany
{
    /// <summary>
    /// Validator for <see cref="CreateCompanyRequest"/>
    /// </summary>
    public class CreateCompanyValidator : Validator<CreateCompanyRequest>
    {
        /// <summary>
        /// Constructor of <see cref="CreateCompanyValidator"/>, register validator rules for <see cref="CreateCompanyRequest"/>
        /// </summary>
        public CreateCompanyValidator()
        {
            WithValidator(MsgCode.ERR_COMPANY_INVALID);
            //RuleFor(x => x.CompanyPid)!.GreaterThan(0);
            RuleFor(x => x.TaxCode).NotNull()!.MaxLength(ErpGeneralCompanyConst.TAX_CODE_MAX_LENGTH);
            RuleFor(x => x.Name).NotNull()!.MaxLength(ErpGeneralCompanyConst.NAME_MAX_LENGTH);
            RuleFor(x => x.Image).NotNull()!.MaxLength(ErpGeneralCompanyConst.IMAGE_MAX_LENGTH);
            RuleFor(x => x.Tel)!.MaxLength(ErpGeneralCompanyConst.TEL_MAX_LENGTH);
            RuleFor(x => x.Email)!.MaxLength(ErpGeneralCompanyConst.EMAIL_MAX_LENGTH);
            RuleFor(x => x.Website)!.MaxLength(ErpGeneralCompanyConst.WEBSITE_MAX_LENGTH);
            RuleFor(x => x.Founder)!.MaxLength(ErpGeneralCompanyConst.FOUNDER_MAX_LENGTH);
            RuleFor(x => x.Ceo).NotNull()!.MaxLength(ErpGeneralCompanyConst.CEO_MAX_LENGTH);
            RuleFor(x => x.CeoImage)!.MaxLength(ErpGeneralCompanyConst.CEO_IMAGE_MAX_LENGTH);
            RuleFor(x => x.CeoEmail)!.MaxLength(ErpGeneralCompanyConst.CEO_EMAIL_MAX_LENGTH);
            RuleFor(x => x.CeoTel)!.MaxLength(ErpGeneralCompanyConst.CEO_TEL_MAX_LENGTH);
            RuleFor(x => x.License)!.MaxLength(ErpGeneralCompanyConst.LICENSE_MAX_LENGTH);
            RuleFor(x => x.CountryId)!.IsConstantCase().MaxLength(ErpGeneralCompanyConst.COUNTRY_ID_MAX_LENGTH);
            RuleFor(x => x.WardId)!.GreaterThan(0);
        }
    }
}