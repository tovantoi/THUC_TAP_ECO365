using _365EJSC.ERP.Application.Requests.Define.WebLocal;
using _365EJSC.ERP.Application.Validators.Define.WebLocal;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocal
{   /// <summary>
    /// Handler for <see cref="GetDetailWebLocalRequests"/>
    /// </summary>
    public class GetDetailWebLocalHandler : IRequestHandler<GetDetailWebLocalRequest, Result<WebLocals>>
    {
        /// <summary>
        /// Repo/// Repository handle data access of <see cref="WebLocals"/>>  /// </summary>
        private readonly IWebLocalSqlRepository webLocalSqlRepository;

        /// <summary>
        /// Constructor of <see cref="GetDetailSampleHandler"/>, inject needed dependency
        /// </summary>
        public GetDetailWebLocalHandler(IWebLocalSqlRepository webLocalSqlRepository)
        {
            this.webLocalSqlRepository = webLocalSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="GetDetailWebLocalRequests"/>, get <see cref="WebLocals"/> from database with id provided in <see cref="GetDetailWebLocalRequests"/>.
        /// Throw not found exception when <see cref="WebLocals"/> with id was not found
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with founded <see cref="Sample"/></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>>
        public async Task<Result<WebLocals>> Handle(GetDetailWebLocalRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request 
            GetDetailWebLocalValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find sample by id provided. If sample not found will throw NotFoundException
            WebLocals? webLocal = await webLocalSqlRepository.FindByIdAsync(request.Id, false, cancellationToken);
            if (webLocal is null) CustomException.ThrowNotFoundException(typeof(WebLocals), MsgCode.ERR_KEY_LOCAL_NOT_FOUND);

            return webLocal;
        }
    }
}