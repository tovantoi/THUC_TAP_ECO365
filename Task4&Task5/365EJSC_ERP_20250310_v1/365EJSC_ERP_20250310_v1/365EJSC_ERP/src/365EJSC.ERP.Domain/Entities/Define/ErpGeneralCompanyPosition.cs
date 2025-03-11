using _365EJSC.ERP.Domain.Abstractions.Aggregates;

namespace _365EJSC.ERP.Domain.Entities.Define
{
    // <summary>
    /// Domain entity with int key type
    /// </summary>
    public class ErpGeneralCompanyPosition : AggregateRoot<int>
    {
        public int? CompanyId { get; set; }
        public int? PositionId { get; set; }
    }
}