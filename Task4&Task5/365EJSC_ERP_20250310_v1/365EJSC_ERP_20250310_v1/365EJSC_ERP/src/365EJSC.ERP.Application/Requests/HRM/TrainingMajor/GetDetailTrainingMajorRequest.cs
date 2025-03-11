using _365EJSC.ERP.Contract.Shared;
using MediatR;
using System.Text.Json.Serialization;
using Entities = _365EJSC.ERP.Domain.Entities.HRM;

namespace _365EJSC.ERP.Application.Requests.HRM.TrainingMajor
{
    /// <summary>
    /// Request to get existed <see cref="TrainingMajor"/> by id from database
    /// </summary>
    public class GetDetailTrainingMajorRequest : IRequest<Result<Entities.TrainingMajor>>
    {
        [JsonIgnore]
        public int? Id { get; set; }
    }
}
