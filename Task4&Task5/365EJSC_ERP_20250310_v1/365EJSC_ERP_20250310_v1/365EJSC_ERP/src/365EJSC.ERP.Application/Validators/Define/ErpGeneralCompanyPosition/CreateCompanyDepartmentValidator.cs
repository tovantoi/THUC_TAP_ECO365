using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyPosition;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompanyPosition
{
    /// <summary>
    /// Validator for <see cref="CreateCompanyPositionRequest"/>
    /// </summary>
    public class CreateCompanyPositionValidator : Validator<CreateCompanyPositionRequest>
    {
        /// <summary>
        /// Constructor of <see cref="CreateCompanyPositionValidator"/>, register validator rules for <see cref="CreateCompanyPositionRequest"/>
        /// </summary>
        public CreateCompanyPositionValidator()
        {
            WithValidator(MsgCode.ERR_COMPANY_POSITION_INVALID);
            RuleFor(x => x.CompanyId)!.NotNull().GreaterThan(0);
            RuleFor(x => x.PositionIds)!.NotNull().Must(list => list.Count > 0 && list.All(id => id > 0));
        }
    }
}