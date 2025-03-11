using _365EJSC.ERP.Application.Requests.HRM.Bank;
using _365EJSC.ERP.Application.Validators.HRM.Bank;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.HRM.Bank
{
    /// <summary>
    /// Handler for <see cref="GetDetailBankRequest"/>
    /// </summary>
    public class GetDetailBankHandler : IRequestHandler<GetDetailBankRequest, Result<HrmBank>>
    {
        private readonly IBankSqlRepository bankSqlRepository;

        public GetDetailBankHandler(IBankSqlRepository bankSqlRepository)
        {
            this.bankSqlRepository = bankSqlRepository;
        }

        public async Task<Result<HrmBank>> Handle(GetDetailBankRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request 
            GetDetailBankValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find bank by id provided. If bank not found will throw NotFoundException
            HrmBank? bank = await bankSqlRepository.FindByIdAsync(request.Id.Value, false, cancellationToken);
            if (bank is null) CustomException.ThrowNotFoundException(typeof(HrmBank), MsgCode.ERR_BANK_ID_NOT_FOUND);

            return bank;
        }
    }
}
