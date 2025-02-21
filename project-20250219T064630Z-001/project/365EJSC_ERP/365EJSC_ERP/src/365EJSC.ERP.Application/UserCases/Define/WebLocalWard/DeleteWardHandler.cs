using _365EJSC.ERP.Application.Requests.Define.WebLocalWard;
using _365EJSC.ERP.Application.Validators.Define.WebLocalWard;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using _365EJSC.ERP.Domain.Entities.Define;
using MediatR;
using System.Data;

namespace _365EJSC.ERP.Application.UserCases.Define.WebLocalWard
{
    public class DeleteWardHandler : IRequestHandler<DeleteWardCommand, Result<object>>
    {
        private readonly IWardSqlRepository wardsqlRepository;
        private readonly ISqlUnitOfWork sqlUnitOfWork;

        public DeleteWardHandler(IWardSqlRepository wardsqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.wardsqlRepository = wardsqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }
        public async Task<Result<object>> Handle(DeleteWardCommand request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            DeleteWardValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find sample base on id provided from database, if sample was not found, throw not found exception.
            // Need tracking to delete sample.
            WebsiteLocalizationWard sample = await wardsqlRepository.FindByIdAsync((int)request.Id, true, cancellationToken);

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
