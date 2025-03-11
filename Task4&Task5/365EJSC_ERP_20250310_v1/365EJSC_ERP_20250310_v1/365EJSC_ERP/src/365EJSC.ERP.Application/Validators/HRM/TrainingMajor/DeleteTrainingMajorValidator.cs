using _365EJSC.ERP.Application.Requests.HRM.TrainingMajor;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;

namespace _365EJSC.ERP.Application.Validators.HRM.TrainingMajor
{
    /// <summary>
    /// Validator for <see cref="DeleteTrainingMajorRequest"/>
    /// </summary>
    public class DeleteTrainingMajorValidator : Validator<DeleteTrainingMajorRequest>
    {
        /// <summary>
        /// Constructor of <see cref="DeleteWebLocalWardValidator"/>, register validator rules for <see cref="DeleteTrainingMajorRequest"/>
        /// </summary>
        public DeleteTrainingMajorValidator()
        {
            WithValidator(MsgCode.ERR_TRAININGMAJOR_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}
