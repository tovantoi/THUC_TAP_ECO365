using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyDepartment;
using _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompanyPosition;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.Define.ErpGeneralCompanyDepartment
{
    /// <summary>
    /// Validator for <see cref="DeleteCompanyDepartmentRequest"/>
    /// </summary>
    public class DeleteCompanyDepartmentValidator : Validator<DeleteCompanyDepartmentRequest>
    {
        /// <summary>
        /// Constructor of <see cref="DeleteCompanyDepartmentValidator"/>, register validator rules for <see cref="DeleteCompanyDepartmentRequest"/>
        /// </summary>
        public DeleteCompanyDepartmentValidator()
        {
            WithValidator(MsgCode.ERR_COMPANY_DEPARTMENT_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}