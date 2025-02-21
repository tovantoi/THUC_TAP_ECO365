using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.Define.WebLocalWards
{
    public class GetDetailWebLocalWardValidator : Validator<GetDetailWebLocalWardRequest>
    {
        public GetDetailWebLocalWardValidator()
        {
            WithValidator(MsgCode.ERR_WARD_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}
