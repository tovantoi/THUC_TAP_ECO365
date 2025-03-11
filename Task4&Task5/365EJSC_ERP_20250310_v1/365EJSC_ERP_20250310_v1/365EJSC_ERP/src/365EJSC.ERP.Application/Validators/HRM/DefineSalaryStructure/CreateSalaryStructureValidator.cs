using _365EJSC.ERP.Application.Requests.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.HRM;

namespace _365EJSC.ERP.Application.Validators.HRM.DefineSalaryStructure
{
    /// <summary>
    /// Validator for <see cref="CreateSalaryStructureRequest"/>
    /// </summary>
    public class CreateSalaryStructureValidator : Validator<CreateSalaryStructureRequest>
    {
        /// <summary>
        /// Constructor of <see cref="CreateSalaryStructureValidator"/>, register validator rules for <see cref="CreateSalaryStructureRequest"/>
        /// </summary>
        public CreateSalaryStructureValidator()
        {
            WithValidator(MsgCode.ERR_SALARY_STRUCTURE_INVALID);
            RuleFor(x => x.Code)!.MaxLength(DefineSalaryStructureConst.CODE_MAX_LENGTH);
            RuleFor(x => x.Name).NotNull().NotEmpty().MaxLength(DefineSalaryStructureConst.NAME_MAX_LENGTH);
        }
    }
}