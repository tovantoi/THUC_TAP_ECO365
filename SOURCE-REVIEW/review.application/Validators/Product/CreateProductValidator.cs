using review.application.Requests.Product;
using review.Contract.DependencyInjection.Extensions;
using review.Contract.Enumerations;
using review.Contract.Validators;
using review.Domain.Constants;

namespace review.application.Validators.Product
{
    /// <summary>
    /// Validator for <see cref="CreateProductCommand"/>
    /// </summary>
    public class CreateProductValidator : Validator<CreateProductCommand>
    {
        /// <summary>
        /// Constructor of <see cref="CreateProductValidator"/>, register validator rules for <see cref="CreateProductCommand"/>
        /// </summary>
        public CreateProductValidator()
        {
            WithValidator(MsgCode.ERR_SAMPLE_INVALID);
            RuleFor(x => x.Name).NotNull()!.NotEmpty().MaxLength(ProductConst.NAME_MAX_LENGTH);
            RuleFor(x => x.Description).NotNull()!.NotEmpty();

        }
    }
}