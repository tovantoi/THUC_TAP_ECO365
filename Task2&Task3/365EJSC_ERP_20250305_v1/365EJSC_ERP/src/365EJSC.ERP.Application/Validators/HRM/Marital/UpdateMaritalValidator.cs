using _365EJSC.ERP.Application.Requests.HRM.Marital;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.HRM;

namespace _365EJSC.ERP.Application.Validators.HRM.Marital
{
    /// <summary>
    /// Validator for <see cref="UpdateMaritalRequest"/>
    /// </summary>
    public class UpdateMaritalValidator : Validator<UpdateMaritalRequest>
    {
        /// <summary>
        /// Constructor of <see cref="UpdateMaritalValidator"/>, register validator rules for <see cref="UpdateMaritalRequest"/>
        /// </summary>
        public UpdateMaritalValidator()
        {
            WithValidator(MsgCode.ERR_MARITAL_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.Name).MaxLength(MaritalConst.NAME_MAX_LENGTH);
        }
    }
}
