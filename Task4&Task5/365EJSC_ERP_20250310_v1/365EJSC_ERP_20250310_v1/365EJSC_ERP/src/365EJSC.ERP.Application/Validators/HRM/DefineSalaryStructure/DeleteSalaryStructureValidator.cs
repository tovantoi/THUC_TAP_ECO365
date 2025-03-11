using _365EJSC.ERP.Application.Requests.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.HRM.DefineSalaryStructure
{
    /// <summary>
    /// Validator for <see cref="DeleteSalaryStructureRequest"/>
    /// </summary>
    public class DeleteSalaryStructureValidator : Validator<DeleteSalaryStructureRequest>
    {
        /// <summary>
        /// Constructor of <see cref="DeleteSalaryStructureValidator"/>, register validator rules for <see cref="DeleteSalaryStructureRequest"/>
        /// </summary>
        public DeleteSalaryStructureValidator()
        {
            WithValidator(MsgCode.ERR_POSITION_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}