using _365EJSC.ERP.Application.Requests.Define.WebLocal;
using _365EJSC.ERP.Application.Requests.HRM.Degree;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.HRM.Degree
{
	public class GetDetailDegreeValidator : Validator<GetDetailDegreeRequest>
	{
		/// <summary>
		/// Constructor of <see cref="GetDetailDegreeValidator"/>, register validator rules for <see cref="GetDetailDegreeRequest"/>
		/// </summary>
		public GetDetailDegreeValidator()
		{
			WithValidator(MsgCode.ERR_DEGREE_INVALID);
			RuleFor(x => x.Id).NotNull().GreaterThan(0);
		}
	}
}
