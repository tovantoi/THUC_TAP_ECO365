using _365EJSC.ERP.Application.Requests.Product.ProductType;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.Product.ProductType
{
    /// <summary>
    /// Validator for <see cref="DeleteProductTypeRequest"/>
    /// </summary>
    public class DeleteProductTypeValidator : Validator<DeleteProductTypeRequest>
    {
        /// <summary>
        /// Constructor of <see cref="DeleteProductTypeValidator"/>, register validator rules for <see cref="DeleteProductTypeRequest"/>
        /// </summary>
        public DeleteProductTypeValidator()
        {
            WithValidator(MsgCode.ERR_PRODUCT_TYPE_INVALID);
            RuleFor(x => x.Id).NotNull()!.GreaterThan(0);
        }
    }
}
