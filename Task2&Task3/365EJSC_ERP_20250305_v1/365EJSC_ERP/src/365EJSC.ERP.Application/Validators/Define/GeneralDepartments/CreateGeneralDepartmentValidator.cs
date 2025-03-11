using _365EJSC.ERP.Application.Requests.Define.GeneralDepartments;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.Define;

namespace _365EJSC.ERP.Application.Validators.Define.GeneralDepartments
{
    /// <summary>
    /// Validator for <see cref="CreateDepartmentRequest"/>
    /// </summary>
    public class CreateGeneralDepartmentValidator : Validator<CreateGeneralDepartmentRequest>
    {
        /// <summary>
        /// Constructor of <see cref="CreateGeneralDepartmentValidator"/>, register validator rules for <see cref="CreateGeneralDepartmentRequest"/>
        /// </summary>
        public CreateGeneralDepartmentValidator()
        {
            WithValidator(MsgCode.ERR_DEPARTMENT_INVALID);

            RuleFor(x => x.DeCode).NotNull()!.NotEmpty().MaxLength(GeneralDepartmentConst.DE_CODE_MAX_LENGTH);
            RuleFor(x => x.DeName).NotNull()!.NotEmpty().MaxLength(GeneralDepartmentConst.DE_NAME_MAX_LENGTH);

            RuleFor(x => x.IsActived).NotNull();
        }
    }
}
