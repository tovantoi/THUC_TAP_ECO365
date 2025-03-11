using _365EJSC.ERP.Application.Requests.Define.ErpGeneralPositions;
using _365EJSC.ERP.Application.Requests.HRM.Degree;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using Entities=_365EJSC.ERP.Domain.Entities.HRM;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.HRM.Degree
{
	public class GetAllDegreeHandler : IRequestHandler<GetAllDegreeRequest, Result<List<Entities.Degree>>>

	{
		/// <summary>
		/// Repository handle data access of <see cref="Degree"/>>
		/// </summary>
		private readonly IDegreeSqlRepository degreeSqlRepository;

		/// <summary>
		/// Constructor of <see cref="GetAllDegreeHandler"/>, inject needed dependency
		/// </summary>
		public GetAllDegreeHandler(IDegreeSqlRepository degreeSqlRepository)
		{
			this.degreeSqlRepository = degreeSqlRepository;
		}

		/// <summary>
		/// Handle <see cref="GetAllDegreeRequest"/>, get all ErpGeneralPosition in database, can skip a number of records and limit record taken
		/// </summary>
		/// <param name="request">Request to handle</param>
		/// <param name="cancellationToken"></param>
		/// <returns><see cref="Result{TModel}"/> with list of <see cref="Degree"/></returns>
		/// <exception cref="Exception"></exception>
		public Task<Result<List<Entities.Degree>>> Handle(GetAllDegreeRequest request, CancellationToken cancellationToken)

		{
			return Task.FromResult<Result<List<Entities.Degree>>>(degreeSqlRepository.FindAll().ToList());
		}
	}
}
