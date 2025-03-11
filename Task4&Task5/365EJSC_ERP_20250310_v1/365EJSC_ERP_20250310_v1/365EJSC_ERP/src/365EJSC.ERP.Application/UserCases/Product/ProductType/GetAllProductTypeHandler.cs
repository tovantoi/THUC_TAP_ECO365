using _365EJSC.ERP.Application.Requests.Product.ProductType;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Product.ProductType;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.Product.ProductType
{
    public class GetAllProductTypeHandler : IRequestHandler<GetAllProductTypeRequest, Result<List<PdProductType>>>
    {
        private readonly IProductTypeSqlRepository productTypeSqlRepository;

        public GetAllProductTypeHandler(IProductTypeSqlRepository productTypeSqlRepository)
        {
            this.productTypeSqlRepository = productTypeSqlRepository;
        }

        public Task<Result<List<PdProductType>>> Handle(GetAllProductTypeRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Result<List<PdProductType>>>(productTypeSqlRepository.FindAll().ToList());
        }
    }
}
