using _365EJSC.ERP.Application.Requests.HRM.EmployeeRole;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.HRM;

namespace _365EJSC.ERP.Application.Validators.HRM.EmployeeRole
{
    public class CreateEmployeeRoleValidator : Validator<CreateEmployeeRoleRequest>
    {
        /// <summary>
        /// Constructor of <see cref="CreateEmployeeRoleValidator"/>, register validator rules for <see cref="CreateEmployeeRoleRequest"/>
        /// </summary>
        public CreateEmployeeRoleValidator()
        {
            WithValidator(MsgCode.ERR_EMPLOYEE_INVALID);
            RuleFor(x => x.Name).NotEmpty().NotNull()!.MaxLength(EmployeeRoleConst.NAME_MAX_LENGTH);
            RuleFor(x => x.Code).NotEmpty().MaxLength(EmployeeRoleConst.NAME_MAX_LENGTH);
        }
    }
}
