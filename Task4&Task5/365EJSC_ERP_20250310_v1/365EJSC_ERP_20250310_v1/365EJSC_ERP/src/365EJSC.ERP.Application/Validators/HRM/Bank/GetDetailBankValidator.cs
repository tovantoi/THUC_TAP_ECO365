using _365EJSC.ERP.Application.Requests.HRM.Bank;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.HRM.Bank
{
    /// <summary>
    /// Validator for <see cref="GetDetailBankRequest"/>
    /// </summary>
    public class GetDetailBankValidator : Validator<GetDetailBankRequest>
    {
        /// <summary>
        /// Constructor of <see cref="GetDetailBankValidator"/>, register validator rules for <see cref="GetDetailBankRequest"/>
        /// </summary>
        public GetDetailBankValidator()
        {
            WithValidator(MsgCode.ERR_BANK_INVALID);
            RuleFor(x => x.Id).NotNull()!.GreaterThan(0);
        }
    }
}
