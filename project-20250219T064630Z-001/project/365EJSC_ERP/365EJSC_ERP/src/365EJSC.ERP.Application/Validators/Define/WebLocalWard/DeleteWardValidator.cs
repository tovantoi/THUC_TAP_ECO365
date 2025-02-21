using _365EJSC.ERP.Application.Requests.Define.WebLocalWard;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.Define.WebLocalWard
{
    public class DeleteWardValidator : Validator<DeleteWardCommand>
    {
        public DeleteWardValidator()
        {
            WithValidator(MsgCode.ERR_WARD_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}
