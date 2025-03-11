using _365EJSC.ERP.Application.Requests.Define.GeneralDepartments;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.Define;

namespace _365EJSC.ERP.Application.Validators.Define.GeneralDepartments
{
    /// <summary>
    /// Validator for <see cref="UpdateGeneralDepartmentRequest"/>
    /// </summary>
    public class UpdateGeneralDepartmentValidator : Validator<UpdateGeneralDepartmentRequest>
    {
        /// <summary>
        /// Constructor of <see cref="UpdateGeneralDepartmentValidator"/>, register validator rules for <see cref="UpdateGeneralDepartmentRequest"/>
        /// </summary>
        /// 
        public UpdateGeneralDepartmentValidator() 
        {
            WithValidator(MsgCode.ERR_DEPARTMENT_INVALID);

            RuleFor(x => x.Id).NotNull().GreaterThan(0);

            RuleFor(x => x.DeCode).NotEmpty()!.MaxLength(GeneralDepartmentConst.DE_CODE_MAX_LENGTH);
            RuleFor(x => x.DeName).NotEmpty()!.MaxLength(GeneralDepartmentConst.DE_NAME_MAX_LENGTH);
        }
    }
}
