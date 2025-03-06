using _365EJSC.ERP.Application.Requests.Define.WebLocal;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.Define;

namespace _365EJSC.ERP.Application.Validators.Define.WebLocal
{   /// <summary>
    /// Validator for <see cref="CreateWebLocalRequests"/>
    /// </summary>
    public class CreateWebLocalValidator : Validator<CreateWebLocalRequest>
    {
        /// <summary>
        /// Constructor of <see cref="CreateWebLocalValidator"/>, register validator rules for <see cref="CreateSampleCommand"/>
        /// </summary>
        public CreateWebLocalValidator()
        {
            WithValidator(MsgCode.ERR_LOCAL_INVALID);
            RuleFor(x => x.Id).NotNull().IsConstantCase().MaxLength(WebLocalConst.KEY_LOCALIZATION_MAX_LENGTH);
            RuleFor(x => x.Localization).MaxLength(WebLocalConst.LOCALIZATION_MAX_LENGTH);
            RuleFor(x => x.IsActived).NotNull();
        }
    }
}