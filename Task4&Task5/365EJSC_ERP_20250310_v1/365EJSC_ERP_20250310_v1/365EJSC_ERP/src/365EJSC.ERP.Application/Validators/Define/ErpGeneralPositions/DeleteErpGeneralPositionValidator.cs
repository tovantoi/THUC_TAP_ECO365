using _365EJSC.ERP.Application.Requests.Define.ErpGeneralPositions;
using _365EJSC.ERP.Application.Requests.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.Define.ErpGeneralPositions
{
    /// <summary>
    /// Validator for <see cref="DeleteErpGeneralPositionRequest"/>
    /// </summary>
    public class DeleteErpGeneralPositionValidator : Validator<DeleteErpGeneralPositionRequest>
    {
        /// <summary>
        /// Constructor of <see cref="DeleteErpGeneralPositionValidator"/>, register validator rules for <see cref="DeleteErpGeneralPositionRequest"/>
        /// </summary>
        public DeleteErpGeneralPositionValidator()
        {
            WithValidator(MsgCode.ERR_POSITION_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}
