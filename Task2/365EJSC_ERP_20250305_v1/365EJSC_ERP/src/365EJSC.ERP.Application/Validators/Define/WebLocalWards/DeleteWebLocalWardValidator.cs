using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.Define.WebLocalWards
{
    /// <summary>
    /// Validator for <see cref="DeleteWebLocalWardRequest"/>
    /// </summary>
    public class DeleteWebLocalWardValidator : Validator<DeleteWebLocalWardRequest>
    {
        /// <summary>
        /// Constructor of <see cref="DeleteWebLocalWardValidator"/>, register validator rules for <see cref="DeleteWebLocalWardRequest"/>
        /// </summary>
        public DeleteWebLocalWardValidator()
        {
            WithValidator(MsgCode.ERR_WARD_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}
