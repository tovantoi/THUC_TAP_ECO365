using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;

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
            List<WebLocalWard> wards;

            if (request.Id == null)
            {              
                return wards =  wardSqlRepository.FindAll().ToList();
            }

            var ward = await wardSqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken,
                    x => x.WebLocalDistrict,
                    x => x.WebLocalDistrict.WebLocalProvince,
                    x => x.WebLocalDistrict.WebLocalProvince.WebLocals);


                wards = new List<WebLocalWard> { ward };

            return Result<List<WebLocalWard>>.Ok(wards);
        }


    }
}
