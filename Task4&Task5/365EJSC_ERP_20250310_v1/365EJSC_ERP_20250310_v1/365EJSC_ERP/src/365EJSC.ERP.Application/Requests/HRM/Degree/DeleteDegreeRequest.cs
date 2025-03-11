using _365EJSC.ERP.Contract.Abstractions;

namespace _365EJSC.ERP.Application.Requests.HRM.Degree
{
	public class DeleteDegreeRequest : ICommand
	{
		public int? Id { get; set; }
	}
}
