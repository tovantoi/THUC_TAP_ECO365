using _365EJSC.ERP.Application.Requests.Define.ErpGeneralPositions;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.Define;

namespace _365EJSC.ERP.Application.Validators.Define.ErpGeneralPositions
{
    /// <summary>
    /// Validator for <see cref="CreateErpGeneralPositionRequest"/>
    /// </summary>
    public class CreateErpGeneralPositionValidator : Validator<CreateErpGeneralPositionRequest>
    {
        /// <summary>
        /// Constructor of <see cref="CreateErpGeneralPositionValidator"/>, register validator rules for <see cref="CreateErpGeneralPositionRequest"/>
        /// </summary>
        public CreateErpGeneralPositionValidator()
        {
            WithValidator(MsgCode.ERR_POSITION_INVALID);
            RuleFor(x => x.Code)!.MaxLength(ErpGeneralPositionConst.CODE_MAX_LENGTH);
            RuleFor(x => x.Name).NotNull()!.NotEmpty().MaxLength(ErpGeneralPositionConst.NAME_MAX_LENGTH);
        }
    }
}
