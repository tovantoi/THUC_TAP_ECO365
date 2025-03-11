using _365EJSC.ERP.Contract.Shared;
using Entities = _365EJSC.ERP.Domain.Entities.HRM;
using MediatR;

namespace _365EJSC.ERP.Application.Requests.HRM.TrainingMajor
{
    /// <summary>
    /// Request to get all existed <see cref="TrainingMajor"/> from database, can limit records or skip a number of records
    /// </summary>
    public class GetAllTrainingMajorRequest : IRequest<Result<List<Entities.TrainingMajor>>>
    {
    }
}
