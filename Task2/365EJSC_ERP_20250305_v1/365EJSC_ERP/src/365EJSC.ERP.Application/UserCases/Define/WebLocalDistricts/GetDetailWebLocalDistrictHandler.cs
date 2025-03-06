using _365EJSC.ERP.Application.Requests.Define.WebLocalDistricts;
using _365EJSC.ERP.Application.Validators.Define.WebLocalDistricts;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalDistricts
{
    /// <summary>
    /// Handler for <see cref="GetDetailWebLocalDistrictRequest"/>
    /// </summary>
    public class GetDetailWebLocalDistrictHandler : IRequestHandler<GetDetailWebLocalDistrictRequest, Result<WebLocalDistrict>>
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
        /// Constructor of <see cref="GetDetailWebLocalDistrictHandler"/>, inject needed dependency
        /// </summary>
        public GetDetailWebLocalDistrictHandler(IWebLocalDistrictSqlRepository districtRepository, IWebLocalProvinceSqlRepository provinceRepository, IWebLocalSqlRepository localizationRepository)
        {
            this.districtRepository = districtRepository;
            this.provinceRepository = provinceRepository;
            this.localizationRepository = localizationRepository;
        }

        /// <summary>
        /// Handle <see cref="GetDetailWebLocalDistrictRequest"/>, get <see cref="WebLocalDistrict"/> from database with id provided in <see cref="GetDetailWebLocalDistrictRequest"/>.
        /// Throw not found exception when <see cref="WebLocalDistrict"/> with id was not found
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with founded <see cref="WebLocalDistrict"/></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>
        public async Task<Result<WebLocalDistrict>> Handle(GetDetailWebLocalDistrictRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            GetDetailWebLocalDistrictValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find district by id provided. If district not found will throw NotFoundException
            return await districtRepository.FindByIdAsync((int)request.Id, false, cancellationToken,
                x => x.WebLocalProvince,
                x => x.WebLocalProvince.WebLocal);
        }
    }
}