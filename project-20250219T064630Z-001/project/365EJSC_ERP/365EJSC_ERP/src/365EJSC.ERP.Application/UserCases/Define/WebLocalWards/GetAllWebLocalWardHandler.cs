using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalWards
{
    public class GetAllWebLocalWardHandler : IRequestHandler<GetAllWebLocalWardRequest, Result<List<WebLocalWard>>>
    {
        private readonly IWebLocalWardSqlRepository wardSqlRepository;
        private readonly IWebLocalDistrictSqlRepository districtSqlRepository;

        public GetAllWebLocalWardHandler(IWebLocalWardSqlRepository wardSqlRepository, IWebLocalDistrictSqlRepository districtSqlRepository)
        {
            this.wardSqlRepository = wardSqlRepository;
            this.districtSqlRepository = districtSqlRepository;
        }

        public async Task<Result<List<WebLocalWard>>> Handle(GetAllWebLocalWardRequest request, CancellationToken cancellationToken)
        {
            // Lấy danh sách Ward
            var wards = wardSqlRepository.FindAll().ToList();

            // Lấy danh sách District theo DistrictId từ danh sách Ward
            var districtIds = wards.Select(w => w.DistrictId).Distinct().ToList();

            var districts = await districtSqlRepository.FindAll(d => districtIds.Contains(d.Id))
                .ToListAsync(cancellationToken); // Thực thi truy vấn trước

            // Chuyển danh sách thành Dictionary
            var districtDict = districts.ToDictionary(d => d.Id);

            // Gán District vào từng Ward
            foreach (var ward in wards)
            {
                if (districtDict.TryGetValue(ward.DistrictId, out var district))
                {
                    ward.WebLocalDistrict = district;
                }
            }

            return Result<List<WebLocalWard>>.Ok(wards);
        }

    }
}
