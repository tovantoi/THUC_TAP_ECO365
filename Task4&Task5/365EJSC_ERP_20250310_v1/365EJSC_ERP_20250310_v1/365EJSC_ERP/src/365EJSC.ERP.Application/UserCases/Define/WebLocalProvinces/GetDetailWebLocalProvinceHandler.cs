using _365EJSC.ERP.Application.Requests.Define.WebLocalProvinces;
using _365EJSC.ERP.Application.Validators.Define.WebLocalProvinces;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalProvinces
{
    /// <summary>
    /// Handler for <see cref="GetDetailWebLocalProvinceRequest"/>
    /// </summary>
    public class GetDetailWebLocalProvinceHandler : IRequestHandler<GetDetailWebLocalProvinceRequest, Result<WebLocalProvince>>
    {
        /// <summary>
        /// Repository handle data access of <see cref="WebLocalProvince"/>>
        /// </summary>
        private readonly IWebLocalProvinceSqlRepository webLocalProvinceSqlRepository;

        /// <summary>
        /// Constructor of <see cref="GetDetailWebLocalProvinceHandler"/>, inject needed dependency
        /// </summary>
        public GetDetailWebLocalProvinceHandler(IWebLocalProvinceSqlRepository webLocalProvinceSqlRepository)
        {
            this.webLocalProvinceSqlRepository = webLocalProvinceSqlRepository;
        }

        /// <summary>
        /// Handle <see cref="GetDetailWebLocalProvinceRequest"/>, get <see cref="WebLocalProvince"/> from database with id provided in <see cref="GetDetailWebLocalProvinceRequest"/>.
        /// Throw not found exception when <see cref="WebLocalProvince"/> with id was not found
        /// </summary>
        /// <param name="request">Request to handle</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="Result{TModel}"/> with founded <see cref="WebLocalProvince"/></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="CustomException"></exception>>
        public async Task<Result<WebLocalProvince>> Handle(GetDetailWebLocalProvinceRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request 
            GetDetailWebLocalProvinceValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find webLocalProvince by id provided. If webLocalProvince not found will throw NotFoundException
            var webLocalProvince = await webLocalProvinceSqlRepository.FindByIdAsync((int)request.Id, false, cancellationToken, x => x.WebLocal);
            if(webLocalProvince is null)
            {
                CustomException.ThrowNotFoundException(typeof(WebLocalProvince), MsgCode.ERR_PROVINCE_ID_NOT_FOUND, WebLocalProvinceConst.MSG_LOCALPROVINCE_ID_NOT_FOUND);
            }
            return webLocalProvince;
        }
    }
}
