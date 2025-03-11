using _365EJSC.ERP.Application.Requests.HRM.Degree;
using _365EJSC.ERP.Application.Validators.HRM.Degree;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using Entities = _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;
using System.Data;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;


namespace _365EJSC.ERP.Application.UserCases.HRM.Degree
{
	/// <summary>
	/// Hand/// Handler for <see cref="CreateDegreeRequests"/>/ </summary>
	public class UpdateDegreeHandler : IRequestHandler<UpdateDegreeRequest, Result<object>>
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
		public UpdateDegreeHandler(IDegreeSqlRepository degreeSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
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
		public async Task<Result<object>> Handle(UpdateDegreeRequest request, CancellationToken cancellationToken)
		{
			// Create validator and validate request
			UpdateDegreeValidator validator = new();
			validator.ValidateAndThrow(request);

			// Find Local base on id provided from database, if Local was not found, throw not found exception.
			// Need tracking to update Local.
			Entities.Degree? degree = await degreeSqlRepository.FindByIdAsync(request.Id, true, cancellationToken);
			if (degree is null) CustomException.ThrowNotFoundException(typeof(Entities.Degree), MsgCode.ERR_DEGREE_ID_NOT_FOUND);

			// Update Local base on data provided in UpdateLocalCommand request.
			// Keep Local original data if request fields is null
			request.MapTo(degree, true);

			// Begin transaction
			using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
			try
			{
				// Mark Local as Updated state
				degreeSqlRepository.Update(degree!);

				// Save Local to database
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
