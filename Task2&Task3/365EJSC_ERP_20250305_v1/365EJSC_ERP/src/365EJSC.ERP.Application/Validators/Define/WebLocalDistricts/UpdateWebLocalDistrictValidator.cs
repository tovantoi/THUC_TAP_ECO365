using _365EJSC.ERP.Application.Requests.Define.WebLocalDistricts;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.Define;

namespace _365EJSC.ERP.Application.Validators.Define.WebLocalDistricts
{
    /// <summary>
    /// Validator for <see cref="UpdateWebLocalDistrictRequest"/>
    /// </summary>
    public class UpdateWebLocalDistrictValidator : Validator<UpdateWebLocalDistrictRequest>
    {
        /// <summary>
        /// Constructor of <see cref="UpdateWebLocalDistrictValidator"/>, register validator rules for <see cref="UpdateWebLocalDistrictRequest"/>
        /// </summary>
        public UpdateWebLocalDistrictValidator()
        {
            WithValidator(MsgCode.ERR_DISTRICT_INVALID);

            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaxLength(WebLocalDistrictConst.NAME_MAX_LENGTH);
            RuleFor(x => x.FullName).NotEmpty().MaxLength(WebLocalDistrictConst.FULL_NAME_MAX_LENGTH);
            RuleFor(x => x.NameEn).NotEmpty().MaxLength(WebLocalDistrictConst.NAME_EN_MAX_LENGTH);
            RuleFor(x => x.FullNameEn).NotEmpty().MaxLength(WebLocalDistrictConst.FULL_NAME_EN_MAX_LENGTH);

            RuleFor(x => x.ProvinceId).GreaterThan(0);
        }
    }

}
