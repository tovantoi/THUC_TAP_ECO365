using _365Architect.Demo.Application.Requests.Samples;
using _365Architect.Demo.Contract.DependencyInjection.Extensions;
using _365Architect.Demo.Contract.Enumerations;
using _365Architect.Demo.Contract.Validators;
using _365Architect.Demo.Domain.Constants;

namespace _365Architect.Demo.Application.Validators.Samples
{
    /// <summary>
    /// Validator for <see cref="CreateSampleCommand"/>
    /// </summary>
    public class CreateSampleValidator : Validator<CreateSampleCommand>
    {
        /// <summary>
        /// Constructor of <see cref="CreateSampleValidator"/>, register validator rules for <see cref="CreateSampleCommand"/>
        /// </summary>
        public CreateSampleValidator()
        {
            WithValidator(MsgCode.ERR_SAMPLE_INVALID);
            RuleFor(x => x.Title).NotNull()!.NotEmpty().MaxLength(SampleConst.TITLE_MAX_LENGTH);
            RuleFor(x => x.Description).NotNull()!.NotEmpty();
            RuleFor(x => x.DueDate).NotNull();
        }
    }
}