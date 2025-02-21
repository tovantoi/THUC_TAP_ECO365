using _365EJSC.ERP.Application.Requests.Define.WebLocalWard;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.Define;

namespace _365EJSC.ERP.Application.Validators.Define.WebLocalWard
{
    public class UpdateWardValidator : Validator<UpdateWardCommand>
    {
        public UpdateWardValidator()
        {
            WithValidator(MsgCode.ERR_WARD_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.Name).MaxLength(WebsiteLocalizationWardConstants.MAX_LENGTH_NAME);

            RuleFor(x => x.NameEn).MaxLength(WebsiteLocalizationWardConstants.MAX_LENGTH_NAME_EN);

            RuleFor(x => x.FullName).MaxLength(WebsiteLocalizationWardConstants.MAX_LENGTH_FULL_NAME);

            RuleFor(x => x.FullNameEn).MaxLength(WebsiteLocalizationWardConstants.MAX_LENGTH_FULL_NAME_EN);

            RuleFor(x => x.Latitude).GreaterThan(0);

            RuleFor(x => x.Longitude).GreaterThan(0);

            RuleFor(x => x.DistrictId).GreaterThan(0);
        }
    }
}
