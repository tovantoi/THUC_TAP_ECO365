using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalWards
{
    /// <summary>
    /// Handler for <see cref="GetAllWebLocalWardRequest"/>
    /// </summary>
    public class GetAllWebLocalWardHandler : IRequestHandler<GetAllWebLocalWardRequest, Result<List<WebLocalWard>>>
    {
        /// <summary>
        /// Repository handling data access of <see cref="WebLocalWard"/>
        /// </summary>
        private readonly IWebLocalWardSqlRepository wardSqlRepository;

        public GetAllWebLocalWardHandler(IWebLocalWardSqlRepository wardSqlRepository)
        {
            this.wardSqlRepository = wardSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="GetAllWebLocalWardRequest"/>, retrieve all <see cref="WebLocalWard"/>
        /// records from the database
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> containing a list of wards</returns>
        public async Task<Result<List<WebLocalWard>>> Handle(GetAllWebLocalWardRequest request, CancellationToken cancellationToken)
        {
            // Retrieve all wards with related district and province data
            var wards = wardSqlRepository
                .FindAll(null, false, x => x.WebLocalDistrict, x => x.WebLocalDistrict.WebLocalProvince, x => x.WebLocalDistrict.WebLocalProvince.WebLocals)
                .ToList();

            // Check if wards exist
            if (wards == null || !wards.Any())
            {
                return new Result<List<WebLocalWard>>
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    IsSuccess = false,
                    MessageCode = MsgCode.ERR_WARD_INVALID,
                };
            }

            // Return success result with ward list
            return Result<List<WebLocalWard>>.Ok(wards);
        }
    }
}
