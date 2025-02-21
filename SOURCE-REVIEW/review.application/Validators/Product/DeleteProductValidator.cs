using review.application.Requests.Product;
using review.Contract.DependencyInjection.Extensions;
using review.Contract.Enumerations;
using review.Contract.Validators;

namespace review.application.Validators.Product
{
    /// <summary>
    /// Validator for <see cref="DeleteProductCommand"/>
    /// </summary>
    public class DeleteProductValidator : Validator<DeleteProductCommand>
    {
        /// <summary>
        /// Constructor of <see cref="DeleteProductValidator"/>, register validator rules for <see cref="DeleteProductCommand"/>
        /// </summary>
        public DeleteProductValidator()
        {
            WithValidator(MsgCode.ERR_SAMPLE_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}