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
    public class GetAllWebLocalWardHandler : IRequestHandler<GetAllWebLocalWardRequest, Result<List<WebLocalWard>>>
    {
        private readonly IWebLocalWardSqlRepository wardSqlRepository;

        public GetAllWebLocalWardHandler(IWebLocalWardSqlRepository wardSqlRepository)
        {
            this.wardSqlRepository = wardSqlRepository;
        }

        public async Task<Result<List<WebLocalWard>>> Handle(GetAllWebLocalWardRequest request, CancellationToken cancellationToken)
        {
            // Lấy tất cả Ward và Include các quan hệ liên quan
            var wards = await wardSqlRepository.FindAll(
                null, // Không có điều kiện lọc (lấy tất cả)
                false, // Không tracking
                x => x.WebLocalDistrict,
                x => x.WebLocalDistrict.WebLocalProvince,
                x => x.WebLocalDistrict.WebLocalProvince.WebLocals
            ).ToListAsync(cancellationToken);

            // Kiểm tra danh sách có dữ liệu không
            if (wards == null || !wards.Any())
            {
                return new Result<List<WebLocalWard>>
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    IsSuccess = false,
                    MessageCode = MsgCode.ERR_WARD_INVALID,
                };
            }

            return Result<List<WebLocalWard>>.Ok(wards);
        }
    }
}
