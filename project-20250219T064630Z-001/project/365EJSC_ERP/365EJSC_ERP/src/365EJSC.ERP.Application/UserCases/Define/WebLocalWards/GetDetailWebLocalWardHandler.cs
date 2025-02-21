using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Application.Validators.Define.WebLocalWards;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalWards
{
    public class GetDetailWebLocalWardHandler : IRequestHandler<GetDetailWebLocalWardRequest, Result<WebLocalWard>>
    {
        private readonly IWebLocalWardSqlRepository wardSqlRepository;
        private readonly IWebLocalDictrictSqlRepository districtSqlRepository;

        public GetDetailWebLocalWardHandler(IWebLocalWardSqlRepository wardSqlRepository, IWebLocalDictrictSqlRepository districtSqlRepository)
        {
            this.wardSqlRepository = wardSqlRepository;
            this.districtSqlRepository = districtSqlRepository;
        }

        public async Task<Result<WebLocalWard>> Handle(GetDetailWebLocalWardRequest request, CancellationToken cancellationToken)
        {
            GetDetailWebLocalWardValidator validator = new();
            validator.ValidateAndThrow(request);


            var ward = await wardSqlRepository
                .FindAll(w => w.Id == request.Id)
                .Include(w => w.WebsiteLocalizationDictrict) 
                .FirstOrDefaultAsync(cancellationToken);


            if (ward == null)
            {
                return new Result<WebLocalWard>
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    IsSuccess = false,
                    MessageCode = MsgCode.ERR_WARD_ID_NOT_FOUND,  
                };
            }
            return Result<WebLocalWard>.Ok(ward);
        }
    }
}
