using _365EJSC.ERP.Application.Requests.Product.ProductType;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.Product.ProductType
{
    /// <summary>
    /// Validator for <see cref="GetDetailProductTypeRequest"/>
    /// </summary>
    public class GetDetailProductTypeValidator : Validator<GetDetailProductTypeRequest>
    {
        /// <summary>
        /// Constructor of <see cref="GetDetailProductTypeValidator"/>, register validator rules for <see cref="GetDetailProductTypeRequest"/>
        /// </summary>
        public GetDetailProductTypeValidator()
        {
            WithValidator(MsgCode.ERR_PRODUCT_TYPE_INVALID);
            RuleFor(x => x.Id).NotNull()!.GreaterThan(0);
        }
    }
}
