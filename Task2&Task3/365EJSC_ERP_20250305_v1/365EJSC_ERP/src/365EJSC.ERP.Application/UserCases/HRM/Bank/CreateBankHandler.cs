using _365EJSC.ERP.Application.Requests.HRM.Bank;
using _365EJSC.ERP.Application.Validators.HRM.Bank;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.HRM.Bank
{
    /// <summary>
    /// Hand/// Handler for <see cref="CreateBankRequest"/>/ </summary>
    public class CreateBankHandler : IRequestHandler<CreateBankRequest, Result<object>>
    {
        private readonly IBankSqlRepository bankSqlRepository;
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        public CreateBankHandler(IBankSqlRepository bankSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.bankSqlRepository = bankSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        public async Task<Result<object>> Handle(CreateBankRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            CreateBankValidator validator = new();
            validator.ValidateAndThrow(request);

            // Create new bank from request
            HrmBank? bank = request.MapTo<HrmBank>();

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Marked bank as Created state
                bankSqlRepository.Add(bank);

                // Save data to database
                await sqlUnitOfWork.SaveChangesAsync(cancellationToken);

                // Commit transaction
                transaction.Commit();

                // Return success result
                return Result<object>.Ok();
            }
            catch (Exception)
            {
                // Rollback transaction if any exception happened, then throw exception
                transaction.Rollback();
                throw;
            }
        }
    }
}
