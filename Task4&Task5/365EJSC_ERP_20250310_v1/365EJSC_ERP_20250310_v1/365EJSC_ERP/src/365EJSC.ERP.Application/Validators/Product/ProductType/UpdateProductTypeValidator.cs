using _365EJSC.ERP.Application.Requests.Product.ProductType;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.Product.ProductType;

namespace _365EJSC.ERP.Application.Validators.Product.ProductType
{
    /// <summary>
    /// Validator for <see cref="UpdateProductTypeRequest"/>
    /// </summary>
    public class UpdateProductTypeValidator : Validator<UpdateProductTypeRequest>
    {
        /// <summary>
        /// Constructor of <see cref="UpdateProductTypeValidator"/>, register validator rules for <see cref="UpdateProductTypeRequest"/>
        /// </summary>
        public UpdateProductTypeValidator()
        {
            WithValidator(MsgCode.ERR_BANK_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.Name).MaxLength(ProductTypeConst.NAME_MAX_LENGTH);
        }
    }
}
