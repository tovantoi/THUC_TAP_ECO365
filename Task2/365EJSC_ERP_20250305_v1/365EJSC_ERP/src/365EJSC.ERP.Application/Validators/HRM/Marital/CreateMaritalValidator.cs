using _365EJSC.ERP.Application.Requests.HRM.Marital;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.HRM;

namespace _365EJSC.ERP.Application.Validators.HRM.Marital
{
    /// <summary>
    /// Validator for <see cref="CreateMaritalRequest"/>
    /// </summary>
    public class CreateMaritalValidator : Validator<CreateMaritalRequest>
    {
        /// <summary>
        /// Constructor of <see cref="CreateMaritalValidator"/>, register validator rules for <see cref="CreateMaritalRequest"/>
        /// </summary>
        public CreateMaritalValidator()
        {
            WithValidator(MsgCode.ERR_MARITAL_INVALID);
            RuleFor(x => x.Name).NotEmpty().NotNull()!.MaxLength(MaritalConst.NAME_MAX_LENGTH);
        }
    }
}
