using _365EJSC.ERP.Application.Requests.Define.WebLocal;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocal
{     /// <summary>
      /// Handler for <see cref="GetAllWebLocalRequest"/>
      /// </summary>
    public class GetAllWebLocalHandler : IRequestHandler<GetAllWebLocalRequest, Result<List<WebLocals>>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="WebLocals"/>>  /// </summary>
        private readonly IWebLocalSqlRepository webLocalSqlRepository;


        /// <summary>
        /// Constructor of <see cref="GetAllWebLocalHandler"/>, inject needed dependency
        /// </summary>
        public GetAllWebLocalHandler(IWebLocalSqlRepository webLocalSqlRepository)
        {
            this.webLocalSqlRepository = webLocalSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="GetAllWebLocalRequest"/>, get all samples in database, can skip a number of records and limit record taken
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with list of <see cref="WebLocals"/></returns>
        /// <exception cref="Exception"></exception>
        public Task<Result<List<WebLocals>>> Handle(GetAllWebLocalRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult<Result<List<WebLocals>>>(webLocalSqlRepository.FindAll().ToList());
        }
    }
}