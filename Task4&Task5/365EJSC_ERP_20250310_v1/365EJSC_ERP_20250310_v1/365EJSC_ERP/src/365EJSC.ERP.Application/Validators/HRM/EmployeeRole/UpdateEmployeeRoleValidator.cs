using _365EJSC.ERP.Application.Requests.HRM.EmployeeRole;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.HRM;

namespace _365EJSC.ERP.Application.Validators.HRM.EmployeeRole
{
    public class UpdateEmployeeRoleValidator : Validator<UpdateEmployeeRoleRequest>
    {
        /// <summary>
        /// Constructor of <see cref="UpdateEmployeeRoleValidator"/>, register validator rules for <see cref="UpdateEmployeeRoleRequest"/>
        /// </summary>
        public UpdateEmployeeRoleValidator()
        {
            WithValidator(MsgCode.ERR_EMPLOYEE_INVALID);
            RuleFor(x => x.Name).MaxLength(EmployeeRoleConst.NAME_MAX_LENGTH);
            RuleFor(x => x.Code).MaxLength(EmployeeRoleConst.NAME_MAX_LENGTH);
        }
    }
}
