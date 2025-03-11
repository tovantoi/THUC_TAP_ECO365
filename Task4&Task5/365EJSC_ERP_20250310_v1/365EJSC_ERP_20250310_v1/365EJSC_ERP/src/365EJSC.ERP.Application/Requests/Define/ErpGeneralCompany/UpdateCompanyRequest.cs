using _365EJSC.ERP.Contract.Abstractions;
using System.Text.Json.Serialization;

namespace _365EJSC.ERP.Application.Requests.Define.ErpGeneralCompany
{
    /// <summary>
    /// Request to update company, contain id, companyPid, taxcode, name, image, tel, email, website
    /// founder, ceo, ceo image, ceo email, ceo tel, lecese, country id, ward id and isactived
    /// </summary>
    public class UpdateCompanyRequest : ICommand
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public int? CompanyPid { get; set; }
        public string? TaxCode { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Tel { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? Founder { get; set; }
        public string? Ceo { get; set; }
        public string? CeoImage { get; set; }
        public string? CeoEmail { get; set; }
        public string? CeoTel { get; set; }
        public string? License { get; set; }
        public string? CountryId { get; set; }
        public int? WardId { get; set; }
        public bool? IsActived { get; set; }
    }
}