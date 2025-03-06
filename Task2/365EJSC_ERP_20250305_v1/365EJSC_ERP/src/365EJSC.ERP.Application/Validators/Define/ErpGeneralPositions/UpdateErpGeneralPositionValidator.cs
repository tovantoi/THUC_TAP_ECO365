using _365EJSC.ERP.Application.Requests.Define.ErpGeneralPositions;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.Define;

namespace _365EJSC.ERP.Application.Validators.Define.ErpGeneralPositions
{
    /// <summary>
    /// Validator for <see cref="UpdateErpGeneralPositionRequest"/>
    /// </summary>
    public class UpdateErpGeneralPositionValidator : Validator<UpdateErpGeneralPositionRequest>
    {
        /// <summary>
        /// Constructor of <see cref="UpdateErpGeneralPositionValidator"/>, register validator rules for <see cref="UpdateErpGeneralPositionRequest"/>
        /// </summary>
        public UpdateErpGeneralPositionValidator()
        {
            WithValidator(MsgCode.ERR_POSITION_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.Code)!.MaxLength(ErpGeneralPositionConst.CODE_MAX_LENGTH);
            RuleFor(x => x.Name)!.MaxLength(ErpGeneralPositionConst.NAME_MAX_LENGTH);
        }
    }
}
