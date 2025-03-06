using _365EJSC.ERP.Application.Requests.Define.WebLocal;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.Define;

namespace _365EJSC.ERP.Application.Validators.Define.WebLocal
{   
    /// <summary>
    /// Validator for <see cref="DeleteWebLocalRequests"/>
    /// </summary>
    public class DeleteWebLocalValidator : Validator<DeleteWebLocalRequest>
    {
        /// <summary>
        /// Constructor of <see cref="DeleteWebLocalValidator"/>, register validator rules for <see cref="DeleteSampleCommand"/>
        /// </summary>
        public DeleteWebLocalValidator()
        {
            WithValidator(MsgCode.ERR_LOCAL_INVALID);
            RuleFor(x => x.Id).NotNull().MaxLength(WebLocalConst.KEY_LOCALIZATION_MAX_LENGTH);
        }
    }
}