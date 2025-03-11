using _365EJSC.ERP.Application.Requests.HRM.Bank;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.HRM;

namespace _365EJSC.ERP.Application.Validators.HRM.Bank
{
    /// <summary>
    /// Validator for <see cref="CreateBankRequest"/>
    /// </summary>
    public class CreateBankValidator : Validator<CreateBankRequest>
    {
        /// <summary>
        /// Constructor of <see cref="CreateBankValidator"/>, register validator rules for <see cref="CreateBankRequest"/>
        /// </summary>
        public CreateBankValidator()
        {
            WithValidator(MsgCode.ERR_BANK_INVALID);
            RuleFor(x => x.Name).NotEmpty().NotNull()!.MaxLength(BankConst.NAME_MAX_LENGTH);
        }
    }
}
