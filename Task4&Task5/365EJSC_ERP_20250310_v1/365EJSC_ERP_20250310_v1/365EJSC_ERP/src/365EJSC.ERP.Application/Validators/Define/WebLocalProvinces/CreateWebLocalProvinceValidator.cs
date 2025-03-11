using _365EJSC.ERP.Application.Requests.Define.WebLocalProvinces;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.Define;

namespace _365EJSC.ERP.Application.Validators.Define.WebLocalProvinces
{
    /// <summary>
    /// Validator for <see cref="CreateWebLocalProvinceRequest"/>
    /// </summary>
    public class CreateWebLocalProvinceValidator : Validator<CreateWebLocalProvinceRequest>
    {
        /// <summary>
        /// Constructor of <see cref="CreateWebLocalProvinceValidator"/>, register validator rules for <see cref="CreateWebLocalProvinceRequest"/>
        /// </summary>
        public CreateWebLocalProvinceValidator()
        {
            WithValidator(MsgCode.ERR_PROVINCE_INVALID);
            RuleFor(x => x.Name).NotNull()!.NotEmpty().MaxLength(WebLocalProvinceConst.NAME_MAX_LENGTH);
            RuleFor(x => x.NameEn).NotNull()!.NotEmpty().MaxLength(WebLocalProvinceConst.NAME_EN_MAX_LENGTH);
            RuleFor(x => x.FullName).NotNull()!.NotEmpty().MaxLength(WebLocalProvinceConst.FULLNAME_MAX_LENGTH);
            RuleFor(x => x.FullNameEn).NotNull()!.NotEmpty().MaxLength(WebLocalProvinceConst.FULLNAME_EN_MAX_LENGTH);
            RuleFor(x => x.Latitude).NotNull()!.GreaterThan(0);
            RuleFor(x => x.Longitude).NotNull()!.GreaterThan(0);
            RuleFor(x => x.KeyLocalization).NotNull()!.NotEmpty().MaxLength(WebLocalProvinceConst.KEY_LOCALIZATION_MAX_LENGTH);
        }
    }
}
