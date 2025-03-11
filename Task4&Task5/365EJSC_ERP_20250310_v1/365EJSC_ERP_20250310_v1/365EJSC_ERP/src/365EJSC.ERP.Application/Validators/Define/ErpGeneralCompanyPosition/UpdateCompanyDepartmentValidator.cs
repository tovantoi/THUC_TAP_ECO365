using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyPosition;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompanyPosition
{
    /// <summary>
    /// Validator for <see cref="UpdateCompanyPositionRequest"/>
    /// </summary>
    public class UpdateCompanyPositionValidator : Validator<UpdateCompanyPositionRequest>
    {
        /// <summary>
        /// Constructor of <see cref="DeleteCompanyPositionValidator"/>, register validator rules for <see cref="UpdateCompanyPositionRequest"/>
        /// </summary>
        public UpdateCompanyPositionValidator()
        {
            WithValidator(MsgCode.ERR_COMPANY_POSITION_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.CompanyId)!.GreaterThan(0);
            RuleFor(x => x.PositionId)!.GreaterThan(0);
        }
    }
}