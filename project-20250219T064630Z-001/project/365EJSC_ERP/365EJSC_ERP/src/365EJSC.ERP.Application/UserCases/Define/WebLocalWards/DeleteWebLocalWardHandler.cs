using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Application.Validators.Define.WebLocalWards;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalWards
{
    public class DeleteWebLocalWardHandler : IRequestHandler<DeleteWebLocalWardRequest, Result<object>>
    {
        private readonly IWebLocalWardSqlRepository wardsqlRepository;
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        public DeleteWebLocalWardHandler(IWebLocalWardSqlRepository wardsqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.wardsqlRepository = wardsqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }
        public async Task<Result<object>> Handle(DeleteWebLocalWardRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            DeleteWebLocalWardValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find sample base on id provided from database, if sample was not found, throw not found exception.
            // Need tracking to delete sample.
            WebLocalWard sample = await wardsqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // Marked sample as Deleted state
                wardsqlRepository.Remove(sample);

                // Save changes to database
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
