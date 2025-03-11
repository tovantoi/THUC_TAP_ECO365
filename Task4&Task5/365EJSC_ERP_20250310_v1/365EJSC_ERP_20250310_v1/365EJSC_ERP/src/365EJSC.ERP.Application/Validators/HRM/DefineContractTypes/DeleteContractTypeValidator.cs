using _365EJSC.ERP.Application.Requests.HRM.DefineContractTypes;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.HRM.DefineContractTypes
{
    public class DeleteContractTypeValidator : Validator<DeleteContractTypeRequest>
    {
        public DeleteContractTypeValidator()
        {
            WithValidator(MsgCode.ERR_CONTRACT_TYPE_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}
