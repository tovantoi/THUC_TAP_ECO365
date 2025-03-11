using _365EJSC.ERP.Application.Requests.Define.WebLocalDistricts;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.Define.WebLocalDistricts
{
    public class DeleteWebLocalDistrictValidator : Validator<DeleteWebLocalDistrictRequest>
    /// <summary>
    /// Constructor of <see cref="DeleteDistrictValidator"/>, register validator rules for <see cref="DeleteWebLocalDistrictRequest"/>
    /// </summary>
    {
        public DeleteWebLocalDistrictValidator() {
            WithValidator(MsgCode.ERR_DISTRICT_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}
