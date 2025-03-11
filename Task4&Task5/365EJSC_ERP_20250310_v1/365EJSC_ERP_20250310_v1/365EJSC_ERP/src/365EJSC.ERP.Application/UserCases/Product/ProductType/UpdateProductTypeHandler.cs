using _365EJSC.ERP.Application.Requests.Product.ProductType;
using _365EJSC.ERP.Application.Validators.Product.ProductType;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
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
    /// <summary>
    ///  Handler for <see cref="UpdateProductTypeRequest"/>/ 
    /// </summary>
    public class UpdateProductTypeHandler : IRequestHandler<UpdateProductTypeRequest, Result<object>>
    {
        private readonly IProductTypeSqlRepository productTypeSqlRepository;
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        public UpdateProductTypeHandler(IProductTypeSqlRepository productTypeSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.productTypeSqlRepository = productTypeSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        /// <summary>
        /// Handle <see cref="UpdateProductTypeRequest"/>, find existing <see cref="HrmMaritals"/> base on id provided in <see cref="UpdateProductTypeRequest"/>,
        /// update founded <see cref="HrmMaritals"/> base on data provided in <see cref="UpdateProductTypeRequest"/> and save to database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with success status</returns>
        /// <exception cref="Exception"></exception>
        /// <exc/// <exception cref="CustomException"></exception>
        public async Task<Result<object>> Handle(UpdateProductTypeRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            UpdateProductTypeValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find producttype base on id provided from database, if producttype was not found, throw not found exception.
            // Need tracking to update Local.
            PdProductType? producttype = await productTypeSqlRepository.FindByIdAsync(request.Id.Value, true, cancellationToken);
            if (producttype is null) CustomException.ThrowNotFoundException(typeof(PdProductType), MsgCode.ERR_PRODUCT_TYPE_ID_NOT_FOUND);

            // Update producttype base on data provided in UpdateProductTypeRequest.
            // Keep producttype original data if request fields is null
            request.MapTo(producttype, true);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Mark producttype as Updated state
                productTypeSqlRepository.Update(producttype!);

                // Save producttype to database
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
