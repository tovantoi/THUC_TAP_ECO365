using _365EJSC.ERP.Application.Requests.HRM.TrainingMajor;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.HRM;

namespace _365EJSC.ERP.Application.Validators.HRM.TrainingMajor
{
    /// <summary>
    /// Validator for <see cref="UpdateTrainingMajorRequest"/>
    /// </summary>
    public class UpdateTrainingMajorValidator : Validator<UpdateTrainingMajorRequest>
    {
        /// <summary>
        /// Constructor of <see cref="UpdateTrainingMajorValidator"/>, register validator rules for <see cref="UpdateTrainingMajorRequest"/>
        /// </summary>
        public UpdateTrainingMajorValidator()
        {
            WithValidator(MsgCode.ERR_TRAININGMAJOR_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.TmName).NotEmpty().MaxLength(TrainingMajorConst.MAX_LENGTH_NAME);

        }
    }
}
