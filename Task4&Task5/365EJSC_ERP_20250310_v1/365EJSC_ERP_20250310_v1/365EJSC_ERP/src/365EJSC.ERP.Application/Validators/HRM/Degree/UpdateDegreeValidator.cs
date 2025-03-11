using _365EJSC.ERP.Application.Requests.HRM.Degree;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.HRM;

namespace _365EJSC.ERP.Application.Validators.HRM.Degree
{
	public class UpdateDegreeValidator : Validator<UpdateDegreeRequest>
	{
		/// <summary>
		/// Constructor of <see cref="UpdateDegreeValidator"/>, register validator rules for <see cref="UpdateDegreeRequest"/>
		/// </summary>
		public UpdateDegreeValidator()
		{
			WithValidator(MsgCode.ERR_LOCAL_INVALID);

			RuleFor(x => x.Id).NotNull().GreaterThan(0);
			RuleFor(x => x.Name).MaxLength(DegreeConst.DEG_NAME_MAX_LENGTH);

		}
	}
}
