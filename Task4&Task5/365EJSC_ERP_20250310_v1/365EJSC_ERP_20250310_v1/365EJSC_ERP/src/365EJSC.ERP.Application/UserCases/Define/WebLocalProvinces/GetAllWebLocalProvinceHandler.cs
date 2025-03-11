using _365EJSC.ERP.Application.Requests.Define.WebLocalProvinces;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;
using System.Collections.Generic;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalProvinces
{
    /// <summary>
    /// Handler for <see cref="GetAllWebLocalProvinceRequest"/>
    /// </summary>
    public class GetAllWebLocalProvinceHandler : IRequestHandler<GetAllWebLocalProvinceRequest, Result<List<WebLocalProvince>>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="WebLocalProvince"/>>
        /// </summary>
        private readonly IWebLocalProvinceSqlRepository webLocalProvinceSqlRepository;

        /// <summary>
        /// Constructor of <see cref="GetAllWebLocalProvinceHandler"/>, inject needed dependency
        /// </summary>
        public GetAllWebLocalProvinceHandler(IWebLocalProvinceSqlRepository webLocalProvinceSqlRepository)
        {
            this.webLocalProvinceSqlRepository = webLocalProvinceSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="GetAllWebLocalProvinceRequest"/>, get all WebLocalProvinces in database, can skip a number of records and limit record taken
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with list of <see cref="WebLocalProvince"/></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<List<WebLocalProvince>>> Handle(GetAllWebLocalProvinceRequest request, CancellationToken cancellationToken)
        {
            var query = webLocalProvinceSqlRepository.FindAll();

            if (request.KeyLocalization != null)
            {
                int provinceIdInt = request.KeyLocalization.GetHashCode();
                query = query.Where(x => x.KeyLocalization == request.KeyLocalization);
            }

            var provinces = await Task.FromResult(query.ToList());

            return Result<List<WebLocalProvince>>.Ok(provinces);
        }
    }
}
