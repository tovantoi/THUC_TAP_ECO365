using _365EJSC.ERP.Application.Requests.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.HRM.DefineSalaryStructure
{
    /// <summary>
    /// Validator for <see cref="GetDetailSalaryStructureRequest"/>
    /// </summary>
    public class GetDetailSalaryStructureValidator : Validator<GetDetailSalaryStructureRequest>
    {
        /// <summary>
        /// Constructor of <see cref="GetDetailSalaryStructureValidator"/>, register validator rules for <see cref="GetDetailSalaryStructureRequest"/>
        /// </summary>
        public GetDetailSalaryStructureValidator()
        {
            WithValidator(MsgCode.ERR_SALARY_STRUCTURE_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}