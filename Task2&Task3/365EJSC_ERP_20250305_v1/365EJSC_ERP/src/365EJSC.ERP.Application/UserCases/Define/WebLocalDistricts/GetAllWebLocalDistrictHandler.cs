using _365EJSC.ERP.Application.Requests.Define.WebLocalDistricts;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalDistricts
{
    /// <summary>
    /// Handler for <see cref="GetAllWebLocalDistrictRequest"/>
    /// </summary>
    public class GetAllWebLocalDistrictHandler : IRequestHandler<GetAllWebLocalDistrictRequest, Result<List<WebLocalDistrict>>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="WebLocalDistrict"/>
        /// </summary>
        private readonly IWebLocalDistrictSqlRepository districtRepository;

        /// <summary>
        /// Repository handle data access of <see cref="WebLocalProvince"/>
        /// </summary>
        private readonly IWebLocalProvinceSqlRepository provinceRepository;

        /// <summary>
        /// Repository handle data access of <see cref="WebLocal"/>
        /// </summary>
        private readonly IWebLocalSqlRepository localizationRepository;


        /// <summary>
        /// Constructor of <see cref="GetAllDistrictsHandler"/>, inject needed dependency
        /// </summary>
        public GetAllWebLocalDistrictHandler(IWebLocalDistrictSqlRepository districtRepository, IWebLocalProvinceSqlRepository provinceRepository, IWebLocalSqlRepository localizationRepository)
        {
            this.districtRepository = districtRepository;
            this.provinceRepository = provinceRepository;
            this.localizationRepository = localizationRepository;
        }

        /// <summary>
        /// Handle <see cref="GetAllWebLocalDistrictRequest"/>, get all districts in database, can skip a number of records and limit record taken
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with list of <see cref="WebLocalDistrict"/></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<List<WebLocalDistrict>>> Handle(GetAllWebLocalDistrictRequest request, CancellationToken cancellationToken)
        {
            var query = districtRepository.FindAll();

            if (request.ProvinceId.HasValue)
            {
                int provinceIdInt = request.ProvinceId.Value.GetHashCode(); 
                query = query.Where(x => x.ProvinceId == provinceIdInt);
            }

            var districts = await Task.FromResult(query.ToList());

            return Result<List<WebLocalDistrict>>.Ok(districts);
        }
    }
}