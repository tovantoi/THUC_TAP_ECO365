using _365EJSC.ERP.Contract.Abstractions;
using Entities = _365EJSC.ERP.Domain.Entities.HRM;


namespace _365EJSC.ERP.Application.Requests.HRM.Degree
{    /// <summary>
	 /// Request to get existed <see cref="Domain.Entities.HRM.Degree"/> by id from database
	 /// </summary>
	public class GetDetailDegreeRequest : IQuery<Entities.Degree>
	{
		public int Id { get; set; }
	}
}
