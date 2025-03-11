using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Application.Validators.Define.WebLocalWards;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;
using System.Net;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalWards
{
    /// <summary>
    /// Handler for <see cref="GetDetailWebLocalWardRequest"/>
    /// </summary>
    public class GetDetailWebLocalWardHandler : IRequestHandler<GetDetailWebLocalWardRequest, Result<WebLocalWard>>
    {
        /// <summary>
        /// Repository handling data access of <see cref="WebLocalWard"/>
        /// </summary>
        private readonly IWebLocalWardSqlRepository wardSqlRepository;


        public GetDetailWebLocalWardHandler(IWebLocalWardSqlRepository wardSqlRepository)
        {
            this.wardSqlRepository = wardSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="GetDetailWebLocalWardRequest"/>, retrieve details of a <see cref="WebLocalWard"/>
        /// based on the provided request
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> containing ward details</returns>
        public async Task<Result<WebLocalWard>> Handle(GetDetailWebLocalWardRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            GetDetailWebLocalWardValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find ward by ID including related district and province data
            var ward = await wardSqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken, x => x.WebLocalDistrict, x => x.WebLocalDistrict.WebLocalProvince, x => x.WebLocalDistrict.WebLocalProvince.WebLocal);

            // Check if ward exists
            if (ward == null)
            {
                return new Result<WebLocalWard>
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    IsSuccess = false,
                    MessageCode = MsgCode.ERR_WARD_ID_NOT_FOUND,
                };
            }

            // Return success result with ward details
            return Result<WebLocalWard>.Ok(ward);
        }
    }
}