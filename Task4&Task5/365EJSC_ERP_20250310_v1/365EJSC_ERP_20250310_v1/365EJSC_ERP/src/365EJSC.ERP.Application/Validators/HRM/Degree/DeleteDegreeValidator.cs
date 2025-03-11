using _365EJSC.ERP.Application.Requests.HRM.Degree;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.HRM.Degree
{
	public class DeleteDegreeValidator : Validator<DeleteDegreeRequest>
	{
		/// <summary>
		/// Constructor of <see cref="DeleteDegreeValidator"/>, register validator rules for <see cref="DeleteDegreeRequest"/>
		/// </summary>
		public DeleteDegreeValidator()
		{
			WithValidator(MsgCode.ERR_DEGREE_INVALID);
			RuleFor(x => x.Id).NotNull().GreaterThan(0);
		}
	}
}
