using _365EJSC.ERP.Application.Requests.Define.WebLocalDistricts;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.Define;

namespace _365EJSC.ERP.Application.Validators.Define.WebLocalDistricts
{
    /// <summary>
    /// Validator for <see cref="CreateWebLocalDistrictRequest"/>
    /// </summary>
    public class CreateWebLocalDistrictValidator : Validator<CreateWebLocalDistrictRequest>
    {
        /// <summary>
        /// Constructor of <see cref="CreateWebLocalDistrictValidator"/>, register validator rules for <see cref="CreateWebLocalDistrictRequest"/>
        /// </summary>
        public CreateWebLocalDistrictValidator()
        {
            WithValidator(MsgCode.ERR_DISTRICT_INVALID);

            RuleFor(x => x.Name).NotNull()!.NotEmpty().MaxLength(WebLocalDistrictConst.NAME_MAX_LENGTH);
            RuleFor(x => x.FullName).NotNull()!.NotEmpty().MaxLength(WebLocalDistrictConst.FULL_NAME_MAX_LENGTH);
            RuleFor(x => x.NameEn).NotNull()!.NotEmpty().MaxLength(WebLocalDistrictConst.NAME_EN_MAX_LENGTH);
            RuleFor(x => x.FullNameEn).NotNull()!.NotEmpty().MaxLength(WebLocalDistrictConst.FULL_NAME_EN_MAX_LENGTH);

            RuleFor(x => x.Latitude).NotNull();
            RuleFor(x => x.Longitude).NotNull();

            RuleFor(x => x.ProvinceId).NotNull().GreaterThan(0);
        }
    }

}
