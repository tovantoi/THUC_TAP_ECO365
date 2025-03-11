using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompany
{
    /// <summary>
    /// Validator for <see cref="DeleteCompanyRequest"/>
    /// </summary>
    public class DeleteCompanyValidator : Validator<DeleteCompanyRequest>
    {
        /// <summary>
        /// Constructor of <see cref="DeleteCompanyValidator"/>, register validator rules for <see cref="DeleteCompanyRequest"/>
        /// </summary>
        public DeleteCompanyValidator()
        {
            WithValidator(MsgCode.ERR_COMPANY_INVALID);
            RuleFor(x => x.Id).NotNull()!.GreaterThan(0);
        }
    }
}