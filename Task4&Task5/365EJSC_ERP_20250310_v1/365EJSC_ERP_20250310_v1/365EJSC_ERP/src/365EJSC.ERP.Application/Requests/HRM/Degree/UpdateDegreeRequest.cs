using _365EJSC.ERP.Contract.Abstractions;
using System.Text.Json.Serialization;

namespace _365EJSC.ERP.Application.Requests.HRM.Degree
{
	public class UpdateDegreeRequest : ICommand
	{
		[JsonIgnore]
		public int Id { get; set; }
		public string? Name { get; set; }
	}
}
