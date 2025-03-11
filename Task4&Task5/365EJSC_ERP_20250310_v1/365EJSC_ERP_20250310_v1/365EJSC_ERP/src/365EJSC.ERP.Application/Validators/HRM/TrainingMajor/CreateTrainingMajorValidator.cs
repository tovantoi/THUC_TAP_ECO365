using _365EJSC.ERP.Application.Requests.HRM.TrainingMajor;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.HRM;

namespace _365EJSC.ERP.Application.Validators.HRM.TrainingMajor
{
    /// <summary>
    /// Validator for <see cref="CreateTrainingMajorRequest"/>
    /// </summary>
    public class CreateTrainingMajorValidator : Validator<CreateTrainingMajorRequest>
    {
        /// <summary>
        /// Constructor of <see cref="CreateTrainingMajorValidator"/>, register validator rules for <see cref="CreateTrainingMajorRequest"/>
        /// </summary>
        public CreateTrainingMajorValidator()
        {
            WithValidator(MsgCode.ERR_TRAININGMAJOR_INVALID);
            RuleFor(x => x.TmName).NotNull()!.NotEmpty().MaxLength(TrainingMajorConst.MAX_LENGTH_NAME);
        }
    }
}
