using _365Architect.Demo.Application.Requests.Samples;
using _365Architect.Demo.Contract.DependencyInjection.Extensions;
using _365Architect.Demo.Contract.Enumerations;
using _365Architect.Demo.Contract.Validators;

namespace _365Architect.Demo.Application.Validators.Samples
{
    /// <summary>
    /// Validator for <see cref="DeleteSampleCommand"/>
    /// </summary>
    public class DeleteSampleValidator : Validator<DeleteSampleCommand>
    {
        /// <summary>
        /// Constructor of <see cref="DeleteSampleValidator"/>, register validator rules for <see cref="DeleteSampleCommand"/>
        /// </summary>
        public DeleteSampleValidator()
        {
            WithValidator(MsgCode.ERR_SAMPLE_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}