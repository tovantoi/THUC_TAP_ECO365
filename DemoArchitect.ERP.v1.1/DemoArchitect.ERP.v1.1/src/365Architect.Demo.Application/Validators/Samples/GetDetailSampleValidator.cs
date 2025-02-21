using _365Architect.Demo.Application.Requests.Samples;
using _365Architect.Demo.Contract.DependencyInjection.Extensions;
using _365Architect.Demo.Contract.Enumerations;
using _365Architect.Demo.Contract.Validators;

namespace _365Architect.Demo.Application.Validators.Samples
{
    /// <summary>
    /// Validator for <see cref="GetDetailSampleQuery"/>
    /// </summary>
    public class GetDetailSampleValidator : Validator<GetDetailSampleQuery>
    {
        /// <summary>
        /// Constructor of <see cref="GetDetailSampleValidator"/>, register validator rules for <see cref="GetDetailSampleQuery"/>
        /// </summary>
        public GetDetailSampleValidator()
        {
            WithValidator(MsgCode.ERR_SAMPLE_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
}