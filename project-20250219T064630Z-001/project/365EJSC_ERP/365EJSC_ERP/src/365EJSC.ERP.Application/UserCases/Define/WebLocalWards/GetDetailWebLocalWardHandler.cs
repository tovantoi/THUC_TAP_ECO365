using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Application.Validators.Define.WebLocalWards;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;
using System.Net;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalWards
{
    public class GetDetailWebLocalWardHandler : IRequestHandler<GetDetailWebLocalWardRequest, Result<WebLocalWard>>
    {
        private readonly IWebLocalWardSqlRepository wardSqlRepository;
        private readonly IWebLocalDistrictSqlRepository districtSqlRepository;

        public GetDetailWebLocalWardHandler(IWebLocalWardSqlRepository wardSqlRepository, IWebLocalDistrictSqlRepository districtSqlRepository)
        {
            this.wardSqlRepository = wardSqlRepository;
            this.districtSqlRepository = districtSqlRepository;
        }
        public async Task<Result<WebLocalWard>> Handle(GetDetailWebLocalWardRequest request, CancellationToken cancellationToken)
        {
            // Validate request
            GetDetailWebLocalWardValidator validator = new();
            validator.ValidateAndThrow(request);

            // Tìm kiếm ward và Include các quan hệ
            var ward = await wardSqlRepository.FindByIdAsync(
                (int)request.Id,
                true,
                cancellationToken,
                x => x.WebLocalDistrict,          // Lấy huyện
                x => x.WebLocalDistrict.WebLocalProvince,  // Lấy tỉnh từ huyện
                x => x.WebLocalDistrict.WebLocalProvince.WebLocals // Lấy quốc gia từ tỉnh
            );

            // Kiểm tra nếu không tìm thấy
            if (ward == null)
            {
                return new Result<WebLocalWard>
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    IsSuccess = false,
                    MessageCode = MsgCode.ERR_WARD_ID_NOT_FOUND,
                };
            }

            // Trả về kết quả thành công
            return Result<WebLocalWard>.Ok(ward);
        }

    }
}