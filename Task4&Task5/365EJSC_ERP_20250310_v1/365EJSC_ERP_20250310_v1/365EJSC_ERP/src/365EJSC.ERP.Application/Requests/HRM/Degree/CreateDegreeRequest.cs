using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.HRM.Degree
{
	public class CreateDegreeRequest : ICommand
	{
		public string? Name  { get; set; } 
	}
}
