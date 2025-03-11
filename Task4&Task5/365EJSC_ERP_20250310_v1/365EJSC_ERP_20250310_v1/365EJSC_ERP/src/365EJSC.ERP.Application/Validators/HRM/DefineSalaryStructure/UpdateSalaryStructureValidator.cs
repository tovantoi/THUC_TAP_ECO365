using _365EJSC.ERP.Application.Requests.HRM.DefineSalaryStructure;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.HRM;

namespace _365EJSC.ERP.Application.Validators.HRM.DefineSalaryStructure
{
    /// <summary>
    /// Validator for <see cref="UpdateSalaryStructureRequest"/>
    /// </summary>
    public class UpdateSalaryStructureValidator : Validator<UpdateSalaryStructureRequest>
    {
        /// <summary>
        /// Constructor of <see cref="UpdateSalaryStructureValidator"/>, register validator rules for <see cref="UpdateSalaryStructureRequest"/>
        /// </summary>
        public UpdateSalaryStructureValidator()
        {
            WithValidator(MsgCode.ERR_SALARY_STRUCTURE_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.Code)!.MaxLength(DefineSalaryStructureConst.CODE_MAX_LENGTH);
            RuleFor(x => x.Name)!.MaxLength(DefineSalaryStructureConst.NAME_MAX_LENGTH);
        }
    }
}