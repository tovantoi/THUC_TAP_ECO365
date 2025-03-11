using _365EJSC.ERP.Application.Requests.HRM.Marital;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.HRM.Marital
{
    /// <summary>
    /// Validator for <see cref="DeleteMaritalRequest"/>
    /// </summary>
    public class DeleteMaritalValidator : Validator<DeleteMaritalRequest>
    {
        /// <summary>
        /// Constructor of <see cref="DeleteMaritalValidator"/>, register validator rules for <see cref="DeleteMaritalRequest"/>
        /// </summary>
        public DeleteMaritalValidator()
        {
            WithValidator(MsgCode.ERR_MARITAL_INVALID);
            RuleFor(x => x.Id).NotNull()!.GreaterThan(0);
        }
    }
}
