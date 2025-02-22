using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.Define;

namespace _365EJSC.ERP.Application.Validators.Define.WebLocalWards
{
    /// <summary>
    /// Validator for <see cref="UpdateWebLocalWardRequest"/>
    /// </summary>
    public class UpdateWebLocalWardValidator : Validator<UpdateWebLocalWardRequest>
    {
        /// <summary>
        /// Constructor of <see cref="UpdateWebLocalWardValidator"/>, register validator rules for <see cref="UpdateWebLocalWardRequest"/>
        /// </summary>
        public UpdateWebLocalWardValidator()
        {
            WithValidator(MsgCode.ERR_WARD_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaxLength(WebLocalWardConst.MAX_LENGTH_NAME);

            RuleFor(x => x.NameEn).NotEmpty().MaxLength(WebLocalWardConst.MAX_LENGTH_NAME_EN);

            RuleFor(x => x.FullName).NotEmpty().MaxLength(WebLocalWardConst.MAX_LENGTH_FULL_NAME);

            RuleFor(x => x.FullNameEn).NotEmpty().MaxLength(WebLocalWardConst.MAX_LENGTH_FULL_NAME_EN);
        }
    }
}
