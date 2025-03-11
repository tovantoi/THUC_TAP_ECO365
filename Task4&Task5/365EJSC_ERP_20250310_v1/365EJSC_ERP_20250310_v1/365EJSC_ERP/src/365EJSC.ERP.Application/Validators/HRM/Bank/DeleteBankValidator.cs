using _365EJSC.ERP.Application.Requests.HRM.Bank;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.HRM.Bank
{
    /// <summary>
    /// Validator for <see cref="DeleteBankRequest"/>
    /// </summary>
    public class DeleteBankValidator : Validator<DeleteBankRequest>
    {
        /// <summary>
        /// Constructor of <see cref="DeleteBankValidator"/>, register validator rules for <see cref="DeleteBankRequest"/>
        /// </summary>
        public DeleteBankValidator()
        {
            WithValidator(MsgCode.ERR_BANK_INVALID);
            RuleFor(x => x.Id).NotNull()!.GreaterThan(0);
        }
    }
}
