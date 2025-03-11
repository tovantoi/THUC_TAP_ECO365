using _365EJSC.ERP.Application.Requests.Product.ProductType;
using _365EJSC.ERP.Application.Validators.Product.ProductType;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Product.ProductType;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.Product.ProductType
{
    /// <summary>
    /// Handler for <see cref="GetDetailProductTypeRequest"/>
    /// </summary>
    public class GetDetailProductTypeHandler : IRequestHandler<GetDetailProductTypeRequest, Result<PdProductType>>
    {
        private readonly IProductTypeSqlRepository productTypeSqlRepository;

        public GetDetailProductTypeHandler(IProductTypeSqlRepository productTypeSqlRepository)
        {
            this.productTypeSqlRepository = productTypeSqlRepository;
        }

        public async Task<Result<PdProductType>> Handle(GetDetailProductTypeRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request 
            GetDetailProductTypeValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find bank by id provided. If producttype not found will throw NotFoundException
            PdProductType? producttype = await productTypeSqlRepository.FindByIdAsync(request.Id.Value, false, cancellationToken);
            if (producttype is null) CustomException.ThrowNotFoundException(typeof(PdProductType), MsgCode.ERR_PRODUCT_TYPE_ID_NOT_FOUND);

            return producttype;
        }
    }
}
