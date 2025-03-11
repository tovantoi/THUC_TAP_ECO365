using _365EJSC.ERP.Application.Requests.HRM.Marital;
using _365EJSC.ERP.Application.Validators.HRM.Marital;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Exceptions;
using _365EJSC.ERP.Contract.Shared;
using _365EJSC.ERP.Domain.Abstractions.Repositories.Sql;
using _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;

namespace _365EJSC.ERP.Application.UserCases.HRM.Marital
{
    /// <summary>
    /// Handler for <see cref="GetDetailMaritalRequest"/>
    /// </summary>
    public class GetDetailMaritalHandler : IRequestHandler<GetDetailMaritalRequest, Result<HrmMarital>>
    {
        private readonly IMaritalSqlRepository hrmMaritalSqlRepository;

        public GetDetailMaritalHandler(IMaritalSqlRepository hrmMaritalSqlRepository)
        {
            this.hrmMaritalSqlRepository = hrmMaritalSqlRepository;
        }
        public async Task<Result<HrmMarital>> Handle(GetDetailMaritalRequest request, CancellationToken cancellationToken)
        {
            // Create validator and validate request 
            GetDetailMaritalValidator validator = new();
            validator.ValidateAndThrow(request);

            // Find marital by id provided. If marital not found will throw NotFoundException
            HrmMarital? marital = await hrmMaritalSqlRepository.FindByIdAsync(request.Id.Value, false, cancellationToken);
            if (marital is null) CustomException.ThrowNotFoundException(typeof(HrmMarital), MsgCode.ERR_MARITAL_ID_NOT_FOUND);

            return marital;
        }
    }
}
