using _365EJSC.ERP.Application.Requests.Product.ProductType;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.Product.ProductType;

namespace _365EJSC.ERP.Application.Validators.Product.ProductType
{
    /// <summary>
    /// Validator for <see cref="CreateProductTypeRequest"/>
    /// </summary>
    public class CreateProductTypeValidator : Validator<CreateProductTypeRequest>
    {
        /// <summary>
        /// Constructor of <see cref="CreateProductTypeValidator"/>, register validator rules for <see cref="CreateProductTypeRequest"/>
        /// </summary>
        public CreateProductTypeValidator()
        {
            WithValidator(MsgCode.ERR_PRODUCT_TYPE_INVALID);
            RuleFor(x => x.Name).NotEmpty().NotNull()!.MaxLength(ProductTypeConst.NAME_MAX_LENGTH);
        }
    }
}
