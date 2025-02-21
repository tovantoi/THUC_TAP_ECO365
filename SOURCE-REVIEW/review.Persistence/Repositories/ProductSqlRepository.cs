using review.Contract.Enumerations;
using review.Contract.Exceptions;
using review.Domain.Abstractions.Repositories.Sql;
using review.Domain.Constants;
using review.Domain.Entities;
using review.Persistence.Repositories.Base;
using System.Linq.Expressions;

namespace review.Persistence.Repositories
{
    /// <summary>
    /// Implementation of IproductRepository
    /// </summary>
    public class ProductSqlRepository : GenericSqlRepository<Product, int>, IProductSqlRepository
    {
        /// <summary>
        /// Implementation of IproductRepository
        /// </summary>
        public ProductSqlRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Override base method, set CreatedAt to now
        /// </summary>
        /// <param name="entity"></param>
        //public void Add(Products entity)
        //{
        //    entity.CreatedAt = DateTime.UtcNow;
        //    base.Add(entity);
        //}

        ///// <summary>
        ///// Override base method, set UpdateAt to now
        ///// </summary>
        ///// <param name="entity"></param>
        //public void Update(Products entity)
        //{
        //    entity.UpdatedAt = DateTime.UtcNow;
        //    base.Update(entity);
        //}

        /// <summary>
        /// Override base method, throw not found exception when entity was not found
        /// </summary>
        /// <param name="id">ID of Domain entity</param>
        /// <param name="cancellationToken"></param>
        /// <param name="includeProperties">Include any relationship if needed</param>
        /// <returns>Domain entity with given id or null if entity with given id not found</returns>
        public async Task<Product?> FindByIdAsync(int id, bool isTracking = false, CancellationToken cancellationToken = default, params Expression<Func<Product, object>>[] includeProperties)
        {
            // Call base method
            Product? product = await base.FindByIdAsync(id, isTracking, cancellationToken, includeProperties);

            // Throw not found exception when product is null
            if (product is null)
                CustomException.ThrowNotFoundException(typeof(Product), MsgCode.ERR_SAMPLE_ID_NOT_FOUND, ProductConst.MSG_PRODUCT_ID_NOT_FOUND);

            // Return founded product
            return product;
        }
    }
}