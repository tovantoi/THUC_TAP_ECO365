using _365EJSC.ERP.Application.Requests.Define.WebLocalProvinces;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.Define;

namespace _365EJSC.ERP.Application.Validators.Define.WebLocalProvinces
{
    /// <summary>
    /// Validator for <see cref="UpdateWebLocalProvinceRequest"/>
    /// </summary>
    public class UpdateWebLocalProvinceValidator : Validator<UpdateWebLocalProvinceRequest>
    {
        /// <summary>
        /// Constructor of <see cref="UpdateWebLocalProvinceValidator"/>, register validator rules for <see cref="UpdateWebLocalProvinceRequest"/>
        /// </summary>
        public UpdateWebLocalProvinceValidator()
        {
            WithValidator(MsgCode.ERR_PROVINCE_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.Name)!.MaxLength(WebLocalProvinceConst.NAME_MAX_LENGTH);
            RuleFor(x => x.NameEn)!.MaxLength(WebLocalProvinceConst.NAME_EN_MAX_LENGTH);
            RuleFor(x => x.FullName)!.MaxLength(WebLocalProvinceConst.FULLNAME_MAX_LENGTH);
            RuleFor(x => x.FullNameEn)!.MaxLength(WebLocalProvinceConst.FULLNAME_EN_MAX_LENGTH);
            RuleFor(x => x.Latitude).GreaterThan(0);
            RuleFor(x => x.Longitude).GreaterThan(0);
        }
    }
}
