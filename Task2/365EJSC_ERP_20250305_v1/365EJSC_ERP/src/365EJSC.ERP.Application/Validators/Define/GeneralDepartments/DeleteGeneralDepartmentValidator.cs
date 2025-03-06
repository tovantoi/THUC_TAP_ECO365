using _365EJSC.ERP.Application.Requests.Define.GeneralDepartments;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.Define.GeneralDepartments
{
    public class DeleteGeneralDepartmentValidator : Validator<DeleteGeneralDepartmentRequest>
    /// <summary>
    /// Constructor of <see cref="DeleteGeneralDepartmentValidator"/>, register validator rules for <see cref="DeleteGeneralDepartmentRequest"/>
    /// </summary>
    {
        public DeleteGeneralDepartmentValidator()
        {
            WithValidator(MsgCode.ERR_DEPARTMENT_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}
