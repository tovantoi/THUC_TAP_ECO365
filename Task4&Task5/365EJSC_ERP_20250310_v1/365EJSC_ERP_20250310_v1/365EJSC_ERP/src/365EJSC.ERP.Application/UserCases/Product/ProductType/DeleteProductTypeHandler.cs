using _365EJSC.ERP.Application.Requests.Product.ProductType;
using _365EJSC.ERP.Application.Validators.Product.ProductType;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Product.ProductType;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.Product.ProductType
{
    public class DeleteProductTypeHandler : IRequestHandler<DeleteProductTypeRequest, Result<object>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="PdProductType"/>>  /// </summary>
        private readonly IProductTypeSqlRepository productTypeSqlRepository;
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        public DeleteProductTypeHandler(IProductTypeSqlRepository productTypeSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.productTypeSqlRepository = productTypeSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Constructor of <see cref="DeleteBankHandler"/>, inject needed dependency
        /// </summary>


        /// <summary>
        /// Handle <see cref="DeleteProductTypeRequest"/>, find existing <see cref="PdProductType"/> base on id provided in <see cref="DeleteProductTypeRequest"/>,
        /// delete founded <see cref="PdProductType"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(DeleteProductTypeRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            DeleteProductTypeValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find bank base on id provided from database, if bank was not found, throw not found exception.
            // Need tracking to delete bank.
            PdProductType? producttype = await productTypeSqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);
            if (producttype is null) CustomException.ThrowNotFoundException(typeof(PdProductType), MsgCode.ERR_PRODUCT_TYPE_ID_NOT_FOUND);
            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Marked sample as Deleted state
                productTypeSqlRepository.Remove(producttype);

                // Save changes to database
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
