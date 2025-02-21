using _365EJSC.ERP.Application.Requests.Define.WebLocalWard;
using _365EJSC.ERP.Application.Validators.Define.WebLocalWard;
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

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalWard
{
    public class CreateWardHandler : IRequestHandler<CreateWardCommand, Result<object>>
    {
        private readonly IWardSqlRepository wardSqlRepository;
        private readonly IDictrictSqlRepository dictrictSqlRepository;

        private readonly ISqlUnitOfWork sqlUnitOfWork;

        public CreateWardHandler(IWardSqlRepository wardSqlRepository, ISqlUnitOfWork sqlUnitOfWork, IDictrictSqlRepository dictrictSqlRepository)
        {
            this.wardSqlRepository = wardSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
            this.dictrictSqlRepository = dictrictSqlRepository;
        }
        public async Task<Result<object>> Handle(CreateWardCommand request, CancellationToken cancellationToken)
        {
            CreateWardValidator validator = new();
            validator.ValidateAndThrow(request);

            WebsiteLocalizationWard? ward = request.MapTo<WebsiteLocalizationWard>();

            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var dictrictIdExists = await dictrictSqlRepository.IsExistAsync(x => x.Id == request.DistrictId);
                if (!dictrictIdExists)
                {
                    var errorMessage = MsgConst.NOT_FOUND_FIND_KEY.FormatMsg(WebsiteLocalizationWardConstants.FIELD_DISTRICT_ID);
                    CustomException.ThrowNotFoundException(typeof(WebsiteLocalizationWard), MsgCode.ERR_DICTRICT_ID_NOT_FOUND, errorMessage);
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
