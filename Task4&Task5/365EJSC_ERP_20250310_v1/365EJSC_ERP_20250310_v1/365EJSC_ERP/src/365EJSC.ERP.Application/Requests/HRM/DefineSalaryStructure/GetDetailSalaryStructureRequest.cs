using _365EJSC.ERP.Contract.Abstractions;
using System.Text.Json.Serialization;
using Entities = _365EJSC.ERP.Domain.Entities.HRM;

namespace _365EJSC.ERP.Application.Requests.HRM.DefineSalaryStructure
{
    /// <summary>
    /// Request to get existed <see cref="Entities.DefineSalaryStructure"/> by id from database
    /// </summary>
    public record GetDetailSalaryStructureRequest : IQuery<Entities.DefineSalaryStructure>
    {
        [JsonIgnore]
        public int? Id { get; set; }
    }
}