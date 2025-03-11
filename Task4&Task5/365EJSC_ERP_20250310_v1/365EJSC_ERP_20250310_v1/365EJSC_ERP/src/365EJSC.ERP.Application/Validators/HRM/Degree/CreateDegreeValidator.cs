

using _365EJSC.ERP.Application.Requests.HRM.Degree;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.HRM;

namespace _365EJSC.ERP.Application.Validators.HRM.Degree
{  /// <summary>
   /// Validator for <see cref="CreateDegreeRequests"/>
   /// </summary>
	public class CreateDegreeValidator : Validator<CreateDegreeRequest>
	{ /// <summary>
	  /// Constructor of <see cref="CreateDegreeValidator"/>, register validator rules for <see cref="CreateDegreeRequest"/>
	  /// </summary>
		public CreateDegreeValidator()
		{
			WithValidator(MsgCode.ERR_DEGREE_INVALID);
			RuleFor(x => x.Name).NotNull()!.NotEmpty().MaxLength(DegreeConst.DEG_NAME_MAX_LENGTH);
		}

	}
}
