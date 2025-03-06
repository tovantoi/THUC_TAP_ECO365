using _365EJSC.ERP.Application.Requests.HRM.Marital;
using _365EJSC.ERP.Application.Validators.HRM.Marital;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql.Base;
using MediatR;
using System.Data;
using _365EJSC.ERP.Domain.Entities.HRM;
namespace _365EJSC.ERP.Application.UserCases.HRM.Marital
{
    /// <summary>
    /// Hand/// Handler for <see cref="CreateMaritalRequest"/>/ </summary>
    public class CreateMaritalHandler : IRequestHandler<CreateMaritalRequest, Result<object>>
    {
        private readonly IMaritalSqlRepository hrmMaritalSqlRepository;
        private readonly ISqlUnitOfWork sqlUnitOfWork;
        public CreateMaritalHandler(IMaritalSqlRepository hrmMaritalSqlRepository, ISqlUnitOfWork sqlUnitOfWork)
        {
            this.hrmMaritalSqlRepository = hrmMaritalSqlRepository;
            this.sqlUnitOfWork = sqlUnitOfWork;
        }

        public async Task<Result<object>> Handle(CreateMaritalRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request
            CreateMaritalValidator validator = new();
            validator.ValidateAndThrow(request);

            // Create new Marital from request
            HrmMarital? marital = request.MapTo<HrmMarital>();

            // Begin transaction
            using IDbTransaction transaction = await sqlUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Marked Marital as Created state
                hrmMaritalSqlRepository.Add(marital);

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
