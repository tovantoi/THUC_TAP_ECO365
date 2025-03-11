using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyPosition;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompanyPosition
{
    /// <summary>
    /// Validator for <see cref="GetDetailCompanyPositionRequest"/>
    /// </summary>
    public class GetDetailCompanyPositionValidator : Validator<GetDetailCompanyPositionRequest>
    {
        /// <summary>
        /// Constructor of <see cref="GetDetailCompanyPositionValidator"/>, register validator rules for <see cref="GetDetailCompanyPositionRequest"/>
        /// </summary>
        public GetDetailCompanyPositionValidator()
        {
            WithValidator(MsgCode.ERR_COMPANY_POSITION_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}