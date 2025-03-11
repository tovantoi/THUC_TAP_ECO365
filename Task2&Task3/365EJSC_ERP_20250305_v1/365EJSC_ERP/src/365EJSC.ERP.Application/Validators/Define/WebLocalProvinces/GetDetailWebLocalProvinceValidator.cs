using _365EJSC.ERP.Application.Requests.Define.WebLocalProvinces;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.Define.WebLocalProvinces
{
    /// <summary>
    /// Validator for <see cref="GetDetailWebLocalProvinceRequest"/>
    /// </summary>
    public class GetDetailWebLocalProvinceValidator : Validator<GetDetailWebLocalProvinceRequest>
    {
        /// <summary>
        /// Constructor of <see cref="GetDetailWebLocalProvinceValidator"/>, register validator rules for <see cref="GetDetailWebLocalProvinceRequest"/>
        /// </summary>
        public GetDetailWebLocalProvinceValidator()
        {
            WithValidator(MsgCode.ERR_PROVINCE_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}
