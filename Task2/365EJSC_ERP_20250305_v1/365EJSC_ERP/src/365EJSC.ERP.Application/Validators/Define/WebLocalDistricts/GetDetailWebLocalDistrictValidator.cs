using _365EJSC.ERP.Application.Requests.Define.WebLocalDistricts;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.Define.WebLocalDistricts
{
    /// <summary>
    /// Validator for <see cref="GetDetailSampleQuery"/>
    /// </summary>
    public class GetDetailWebLocalDistrictValidator : Validator<GetDetailWebLocalDistrictRequest>
    {
        /// <summary>
        /// Constructor of <see cref="GetDetailWebLocalDistrictValidator"/>, register validator rules for <see cref="GetDetailWebLocalDistrictRequest"/>
        /// </summary>
        public GetDetailWebLocalDistrictValidator()
        {
            WithValidator(MsgCode.ERR_DISTRICT_ID_NOT_FOUND);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}
