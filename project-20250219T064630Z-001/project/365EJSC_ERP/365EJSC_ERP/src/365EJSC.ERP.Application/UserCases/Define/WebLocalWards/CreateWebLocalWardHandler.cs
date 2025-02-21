using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Application.Validators.Define.WebLocalWards;
using _365EJSC.ERP.Contract.Constants;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Constants.Define;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalWards
{
    public class CreateWebLocalWardHandler : IRequestHandler<CreateWebLocalWardRequest, Result<object>>
    {
        private readonly IWebLocalWardSqlRepository wardSqlRepository;
        private readonly IWebLocalDistrictSqlRepository dictrictSqlRepository;

        private readonly ISqlUnitOfWork sqlUnitOfWork;

        public CreateWebLocalWardHandler(IWebLocalWardSqlRepository wardSqlRepository, ISqlUnitOfWork sqlUnitOfWork, IWebLocalDistrictSqlRepository dictrictSqlRepository)
        {
            this.wardSqlRepository = wardSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
            this.dictrictSqlRepository = dictrictSqlRepository;
        }
        public async Task<Result<object>> Handle(CreateWebLocalWardRequest request, CancellationToken cancellationToken)
        {
            CreateWebLocalWardValidator validator = new();
            validator.ValidateAndThrow(request);

            WebLocalWard? ward = request.MapTo<WebLocalWard>();

            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var dictrictIdExists = await dictrictSqlRepository.IsExistAsync(x => x.Id == request.DistrictId);
                if (!dictrictIdExists)
                {
                    var errorMessage = MsgConst.NOT_FOUND_FIND_KEY.FormatMsg(WebLocalWardConst.FIELD_DISTRICT_ID);
                    CustomException.ThrowNotFoundException(typeof(WebLocalWard), MsgCode.ERR_DISTRICT_ID_NOT_FOUND, errorMessage);
                }
                wardSqlRepository.Add(ward);

                await sqlUnitOfWork.SaveChangesAsync(cancellationToken);

                transaction.Commit();

                return Result<object>.Ok();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
