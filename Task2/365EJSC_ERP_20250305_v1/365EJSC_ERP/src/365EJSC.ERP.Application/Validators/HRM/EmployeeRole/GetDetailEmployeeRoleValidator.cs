using _365EJSC.ERP.Application.Requests.HRM.EmployeeRole;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.HRM.EmployeeRole
{
    public class GetDetailEmployeeRoleValidator : Validator<GetDetailEmployeeRoleRequest>
    {
        /// <summary>
        /// Constructor of <see cref="GetDetailEmployeeRoleValidator"/>, register validator rules for <see cref="GetDetailEmployeeRoleRequest"/>
        /// </summary>
        public GetDetailEmployeeRoleValidator()
        {
            WithValidator(MsgCode.ERR_EMPLOYEE_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}
