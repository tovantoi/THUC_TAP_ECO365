using _365EJSC.ERP.Application.Requests.HRM.Degree;
using _365EJSC.ERP.Application.Validators.HRM.Degree;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using MediatR;
using Entities = _365EJSC.ERP.Domain.Entities.HRM;
using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Application.UserCases.HRM.Degree
{
	public class GetDetailDegreeHandler : IRequestHandler<GetDetailDegreeRequest, Result<Entities.Degree>>
	{
		/// <summary>
		/// Repo/// Repository handle data access of <see cref="Degree"/>>  /// </summary>
		private readonly IDegreeSqlRepository degreeSqlRepository;

		/// <summary>
		/// Constructor of <see cref="GetDetailDegreeHandler"/>, inject needed dependency
		/// </summary>
		public GetDetailDegreeHandler(IDegreeSqlRepository degreeSqlRepository)
		{
			this.degreeSqlRepository = degreeSqlRepository;
		}

		/// <summary>
		/// Handle <see cref="GetDetailDegreeRequest"/>, get <see cref="WebLocals"/> from database with id provided in <see cref="GetDetailDegreeRequest"/>.
		/// Throw not found exception when <see cref="WebLocals"/> with id was not found
		/// </summary>
		/// <param name="request">Request to handle</param>
		/// <param name="cancellationToken"></param>
		/// <returns><see cref="Result{TModel}"/> with founded <see cref="Sample"/></returns>
		/// <exception cref="Exception"></exception>
		/// <exception cref="CustomException"></exception>>
		public async Task<Result<Entities.Degree>> Handle(GetDetailDegreeRequest request, CancellationToken cancellationToken)
		{
			// Create validator and validate request 
			GetDetailDegreeValidator validator = new();
			validator.ValidateAndThrow(request);

			// Find sample by id provided. If sample not found will throw NotFoundException
			Entities.Degree? degree = await degreeSqlRepository.FindByIdAsync(request.Id, false, cancellationToken);
			if (degree is null) CustomException.ThrowNotFoundException(typeof(Entities.Degree), MsgCode.ERR_DEGREE_ID_NOT_FOUND);

			return degree;
		}
	}
}
