using _365EJSC.ERP.Contract.Abstractions;
using System.Text.Json.Serialization;

namespace _365EJSC.ERP.Application.Requests.HRM.DefineSalaryStructure
{
    /// <summary>
    /// Request to delete SalaryStructure, contain position id
    /// </summary>
    public record DeleteSalaryStructureRequest : ICommand
    {
        [JsonIgnore]
        public int? Id { get; set; }
    }
}