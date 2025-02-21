using review.application.Requests.Product;
using review.Contract.DependencyInjection.Extensions;
using review.Contract.Enumerations;
using review.Contract.Validators;
using review.Domain.Constants;

namespace review.application.Validators.Product
{
    /// <summary>
    /// Validator for <see cref="UpdateProductCommand"/>
    /// </summary>
    public class UpdateProductValidator : Validator<UpdateProductCommand>
    {
        /// <summary>
        /// Constructor of <see cref="UpdateProductValidator"/>, register validator rules for <see cref="UpdateProductCommand"/>
        /// </summary>
        public UpdateProductValidator()
        {
            WithValidator(MsgCode.ERR_SAMPLE_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.Name).NotNull()!.NotEmpty().MaxLength(ProductConst.NAME_MAX_LENGTH);
            RuleFor(x => x.Description).NotNull()!.NotEmpty();
        }
    }
}