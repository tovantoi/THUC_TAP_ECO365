using _365EJSC.ERP.Application.Requests.Define.WebLocalWard;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalWard
{
    public class GetAllWardHandler : IRequestHandler<GetAllWardQuery, Result<List<WebsiteLocalizationWard>>>
    {
        private readonly IWardSqlRepository wardSqlRepository;
        private readonly IDictrictSqlRepository districtSqlRepository;

        public GetAllWardHandler(IWardSqlRepository wardSqlRepository, IDictrictSqlRepository districtSqlRepository)
        {
            wardSqlRepository = wardSqlRepository;
            districtSqlRepository = districtSqlRepository;
        }

        public async Task<Result<List<WebsiteLocalizationWard>>> Handle(GetAllWardQuery request, CancellationToken cancellationToken)
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
                    ward.WebsiteLocalizationDictrict = district;
                }
            }

            return Result<List<WebsiteLocalizationWard>>.Ok(wards);
        }

    }
}
