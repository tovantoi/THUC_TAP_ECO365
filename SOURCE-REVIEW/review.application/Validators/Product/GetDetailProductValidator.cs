using review.application.Requests.Product;
using review.Contract.DependencyInjection.Extensions;
using review.Contract.Enumerations;
using review.Contract.Validators;

namespace review.application.Validators.Product
{
    /// <summary>
    /// Validator for <see cref="GetDetailProductQuery"/>
    /// </summary>
    public class GetDetailProductValidator : Validator<GetDetailProductQuery>
    {
        /// <summary>
        /// Constructor of <see cref="GetDetailProductValidator"/>, register validator rules for <see cref="GetDetailProductQuery"/>
        /// </summary>
        public GetDetailProductValidator()
        {
            WithValidator(MsgCode.ERR_SAMPLE_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}