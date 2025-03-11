namespace _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany.DTO
{
    public class CompanyDTO
    {
        public int Id { get; set; }
        public int CompanyPid { get; set; }
        public string TaxCode { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string? Tel { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? Founder { get; set; }
        public string Ceo { get; set; }
        public string? CeoImage { get; set; }
        public string? CeoEmail { get; set; }
        public string? CeoTel { get; set; }
        public string? License { get; set; }
        public string? CountryId { get; set; }
        public int? WardId { get; set; }
        public List<DepartmentDTO> Departments { get; set; } = new List<DepartmentDTO>();
        public List<PositionDTO> Positions { get; set; } = new List<PositionDTO>();
    }
}