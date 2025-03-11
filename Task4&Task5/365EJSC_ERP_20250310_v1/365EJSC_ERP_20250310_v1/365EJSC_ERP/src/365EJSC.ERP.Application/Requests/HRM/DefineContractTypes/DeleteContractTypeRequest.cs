using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.HRM.DefineContractTypes
{
    public record DeleteContractTypeRequest : ICommand
    {
        public int? Id { get; set; }
    }
}
