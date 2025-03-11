using _365EJSC.ERP.Application.Requests.HRM.DefineContractTypes;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.HRM;

namespace _365EJSC.ERP.Application.Validators.HRM.DefineContractTypes
{
    public class CreateContractTypeValidator : Validator<CreateContractTypeRequest>
    {
        public CreateContractTypeValidator()
        {
            WithValidator(MsgCode.ERR_CONTRACT_TYPE_INVALID);
            RuleFor(x => x.Code)!.MaxLength(ContractTypeConst.CODE_MAX_LENGTH);
            RuleFor(x => x.Name).NotNull()!.NotEmpty().MaxLength(ContractTypeConst.NAME_MAX_LENGTH);
        }
    }
}
