using _365EJSC.ERP.Application.Requests.Product.ProductType;
using _365EJSC.ERP.Application.Validators.Product.ProductType;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Product.ProductType;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.Product.ProductType
{
    /// <summary>
    /// Hand/// Handler for <see cref="CreateProductTypeRequest"/>/ </summary>
    public class CreateProductTypeHandler : IRequestHandler<CreateProductTypeRequest, Result<object>>
    {
        private readonly IProductTypeSqlRepository productTypeSqlRepository;
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        public CreateProductTypeHandler(IProductTypeSqlRepository productTypeSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.productTypeSqlRepository = productTypeSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        public async Task<Result<object>> Handle(CreateProductTypeRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            CreateProductTypeValidator validator = new();
            validator.ValidateAndThrow(request);

            // Create new producttype from request
            PdProductType? producttype = request.MapTo<PdProductType>();

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Marked producttype as Created state
                productTypeSqlRepository.Add(producttype);

                // Save data to database
                await sqlUnitOfWork.SaveChangesAsync(cancellationToken);

                // Commit transaction
                transaction.Commit();

                // Return success result
                return Result<object>.Ok();
            }
            catch (Exception)
            {
                // Rollback transaction if any exception happened, then throw exception
                transaction.Rollback();
                throw;
            }
        }
    }
}
