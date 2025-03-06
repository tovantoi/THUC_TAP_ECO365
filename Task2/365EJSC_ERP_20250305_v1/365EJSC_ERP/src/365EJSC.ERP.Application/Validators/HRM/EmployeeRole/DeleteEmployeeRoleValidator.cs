using _365EJSC.ERP.Application.Requests.HRM.EmployeeRole;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.HRM.EmployeeRole
{
    public class DeleteEmployeeRoleValidator : Validator<DeleteEmployeeRoleRequest>
    {
        /// <summary>
        /// Constructor of <see cref="DeleteEmployeeRoleValidator"/>, register validator rules for <see cref="DeleteEmployeeRoleRequest"/>
        /// </summary>
        public DeleteEmployeeRoleValidator()
        {
            WithValidator(MsgCode.ERR_EMPLOYEE_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}
