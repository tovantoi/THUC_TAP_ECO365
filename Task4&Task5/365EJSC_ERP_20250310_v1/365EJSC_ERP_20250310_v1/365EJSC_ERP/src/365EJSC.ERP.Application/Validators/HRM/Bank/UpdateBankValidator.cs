using _365EJSC.ERP.Application.Requests.HRM.Bank;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.HRM;

namespace _365EJSC.ERP.Application.Validators.HRM.Bank
{
    /// <summary>
    /// Validator for <see cref="UpdateBankRequest"/>
    /// </summary>
    public class UpdateBankValidator : Validator<UpdateBankRequest>
    {
        /// <summary>
        /// Constructor of <see cref="UpdateBankValidator"/>, register validator rules for <see cref="UpdateBankRequest"/>
        /// </summary>
        public UpdateBankValidator()
        {
            WithValidator(MsgCode.ERR_BANK_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.Name).MaxLength(BankConst.NAME_MAX_LENGTH);
        }
    }
}
