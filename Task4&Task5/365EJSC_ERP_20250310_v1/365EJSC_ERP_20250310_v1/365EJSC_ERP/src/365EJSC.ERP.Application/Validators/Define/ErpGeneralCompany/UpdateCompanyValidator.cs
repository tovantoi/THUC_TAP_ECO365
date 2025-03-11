using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.Define;

namespace _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompany
{
    /// <summary>
    /// Validator for <see cref="UpdateCompanyRequest"/>
    /// </summary>
    public class UpdateCompanyValidator : Validator<UpdateCompanyRequest>
    {
        /// <summary>
        /// Constructor of <see cref="UpdateCompanyValidator"/>, register validator rules for <see cref="UpdateCompanyRequest"/>
        /// </summary>
        public UpdateCompanyValidator()
        {
            WithValidator(MsgCode.ERR_COMPANY_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            //RuleFor(x => x.CompanyPid).GreaterThan(0);
            RuleFor(x => x.TaxCode)!.MaxLength(ErpGeneralCompanyConst.TAX_CODE_MAX_LENGTH);
            RuleFor(x => x.Name)!.MaxLength(ErpGeneralCompanyConst.NAME_MAX_LENGTH);
            RuleFor(x => x.Image)!.MaxLength(ErpGeneralCompanyConst.IMAGE_MAX_LENGTH);
            RuleFor(x => x.Tel)!.MaxLength(ErpGeneralCompanyConst.TEL_MAX_LENGTH);
            RuleFor(x => x.Email)!.MaxLength(ErpGeneralCompanyConst.EMAIL_MAX_LENGTH);
            RuleFor(x => x.Website)!.MaxLength(ErpGeneralCompanyConst.WEBSITE_MAX_LENGTH);
            RuleFor(x => x.Founder)!.MaxLength(ErpGeneralCompanyConst.FOUNDER_MAX_LENGTH);
            RuleFor(x => x.Ceo)!.MaxLength(ErpGeneralCompanyConst.CEO_MAX_LENGTH);
            RuleFor(x => x.CeoImage)!.MaxLength(ErpGeneralCompanyConst.CEO_IMAGE_MAX_LENGTH);
            RuleFor(x => x.CeoEmail)!.MaxLength(ErpGeneralCompanyConst.CEO_EMAIL_MAX_LENGTH);
            RuleFor(x => x.CeoTel)!.MaxLength(ErpGeneralCompanyConst.CEO_TEL_MAX_LENGTH);
            RuleFor(x => x.License)!.MaxLength(ErpGeneralCompanyConst.LICENSE_MAX_LENGTH);
            RuleFor(x => x.CountryId)!.IsConstantCase().MaxLength(ErpGeneralCompanyConst.COUNTRY_ID_MAX_LENGTH);
            RuleFor(x => x.WardId).GreaterThan(0);
        }
    }
}