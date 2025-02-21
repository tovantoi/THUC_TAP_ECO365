using _365Architect.Demo.Application.Requests.Samples;
using _365Architect.Demo.Contract.DependencyInjection.Extensions;
using _365Architect.Demo.Contract.Enumerations;
using _365Architect.Demo.Contract.Validators;
using _365Architect.Demo.Domain.Constants;

namespace _365Architect.Demo.Application.Validators.Samples
{
    /// <summary>
    /// Validator for <see cref="UpdateSampleCommand"/>
    /// </summary>
    public class UpdateSampleValidator : Validator<UpdateSampleCommand>
    {
        /// <summary>
        /// Constructor of <see cref="UpdateSampleValidator"/>, register validator rules for <see cref="UpdateSampleCommand"/>
        /// </summary>
        public UpdateSampleValidator()
        {
            WithValidator(MsgCode.ERR_SAMPLE_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.Title)!.NotEmpty().MaxLength(SampleConst.TITLE_MAX_LENGTH);
            RuleFor(x => x.Description)!.NotEmpty();
        }
    }
}