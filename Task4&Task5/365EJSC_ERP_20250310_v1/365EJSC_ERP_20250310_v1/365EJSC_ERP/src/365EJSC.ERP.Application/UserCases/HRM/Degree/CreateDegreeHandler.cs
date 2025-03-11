using _365EJSC.ERP.Application.Requests.HRM.Degree;
using _365EJSC.ERP.Application.Validators.HRM.Degree;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using Entities = _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.HRM.Degree
{
	/// <summary>
	/// Hand/// Handler for <see cref="CreateDegreeRequests"/>/ </summary>
	public class CreateDegreeHandler : IRequestHandler<CreateDegreeRequest, Result<object>>
	{
		/// <summary>
		/// Repo/// Repository handle data access of <see cref="Degree"/>>  /// </summary>
		private readonly IDegreeSqlRepository degreeSqlRepository;
		/// <summary>
		/// Unit of work to handle transaction
		/// </summary>
		private readonly ISqlUnitOfWork sqlUnitOfWork;

		/// <summary>
		/// Constructor of <see cref="CreateDegreeHandler"/>, inject needed dependency
		/// </summary>
		public CreateDegreeHandler(IDegreeSqlRepository degreeSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
		{
			this.degreeSqlRepository = degreeSqlRepository;
			this.sqlUnitOfWork = sqlUnitOfWork;
		}
		/// <summary>
		/// Handle <see cref="CreateDegreeRequest"/>, create new <see cref="Degree"/> base on data <see cref="CreateDegreeRequest"/>
		/// and save to database
		/// </summary>
		/// <param name="request">Request to handle</param>
		/// <param name="cancellationToken"></param>
		/// <returns><see cref="Result{TModel}"/> with success status</returns>
		/// <exception cref="Exception"></exception>
		public async Task<Result<object>> Handle(CreateDegreeRequest request, CancellationToken cancellationToken)
		{
			// Create validator and validate request
			CreateDegreeValidator validator = new();
			validator.ValidateAndThrow(request);

			// Create new webLocal from request

			Entities.Degree? degree = request.MapTo<Entities.Degree>();

			// Begin transaction
			using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
			try
			{
				// Marked sample as Created state
				degreeSqlRepository.Add(degree);

				// Save data to database
				await sqlUnitOfWork.SaveChangesAsync(cancellationToken);

				// Commit transaction
				transaction.Commit();

				// Return success result
				return Result<object>.Ok();
			}
			catch (Exception ex)
			{
				// Rollback transaction if any exception happened, then throw exception
				transaction.Rollback();
				throw;
			}
		}
	}
}
