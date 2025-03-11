using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.Define.WebLocalDistricts
{
    public class DeleteWebLocalDistrictRequest : ICommand
    {
        public int Id { get; set; }
    }
}
