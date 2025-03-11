using _365EJSC.ERP.Application.Requests.Define.ErpGeneralPositions;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.Define.ErpGeneralPositions
{
    /// <summary>
    /// Validator for <see cref="GetDetailErpGeneralPositionRequest"/>
    /// </summary>
    public class GetDetailErpGeneralPositionValidator : Validator<GetDetailErpGeneralPositionRequest>
    {
        /// <summary>
        /// Constructor of <see cref="GetDetailErpGeneralPositionValidator"/>, register validator rules for <see cref="GetDetailErpGeneralPositionRequest"/>
        /// </summary>
        public GetDetailErpGeneralPositionValidator()
        {
            WithValidator(MsgCode.ERR_POSITION_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}
