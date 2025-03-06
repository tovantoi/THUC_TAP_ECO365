using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompany
{
    /// <summary>
    /// Validator for <see cref="GetDetailCompanyQuery"/>
    /// </summary>
    public class GetDetailCompanyValidator : Validator<GetDetailCompanyRequest>
    {
        /// <summary>
        /// Constructor of <see cref="GetDetailCompanyValidator"/>, register validator rules for <see cref="GetDetailCompanyQuery"/>
        /// </summary>
        public GetDetailCompanyValidator()
        {
            WithValidator(MsgCode.ERR_COMPANY_INVALID);
            RuleFor(x => x.Id).NotNull()!.GreaterThan(0);
        }
    }
}