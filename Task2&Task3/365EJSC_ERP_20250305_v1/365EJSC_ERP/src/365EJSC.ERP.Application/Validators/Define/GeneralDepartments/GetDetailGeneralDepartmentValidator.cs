using _365EJSC.ERP.Application.Requests.Define.GeneralDepartments;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.Define.GeneralDepartments
{
    /// <summary>
    /// Validator for <see cref="UpdateGeneralDepartmentRequest"/>
    /// </summary>
    public class GetDetailGeneralDepartmentValidator : Validator<GetDetailGeneralDepartmentRequest>
    {
        /// <summary>
        /// Constructor of <see cref="GetDetailGeneralDepartmentValidator"/>, register validator rules for <see cref="GetDetailGeneralDepartmentRequest"/>
        /// </summary>
        public GetDetailGeneralDepartmentValidator()
        {
            WithValidator(MsgCode.ERR_DEPARTMENT_INVALID);

            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}
