using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyDepartment;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.Define;

namespace _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompanyDepartment
{
    /// <summary>
    /// Validator for <see cref="CreateCompanyDepartmentRequest"/>
    /// </summary>
    public class CreateCompanyDepartmentValidator : Validator<CreateCompanyDepartmentRequest>
    {
        /// <summary>
        /// Constructor of <see cref="CreateCompanyDepartmentValidator"/>, register validator rules for <see cref="CreateCompanyDepartmentRequest"/>
        /// </summary>
        public CreateCompanyDepartmentValidator()
        {
            WithValidator(MsgCode.ERR_COMPANY_DEPARTMENT_INVALID);
            RuleFor(x => x.CompanyId)!.NotNull().GreaterThan(0);
            RuleFor(x => x.DepartmentIds)!.NotNull().Must(list => list.Count > 0 && list.All(id => id > 0));
        }
    }
}