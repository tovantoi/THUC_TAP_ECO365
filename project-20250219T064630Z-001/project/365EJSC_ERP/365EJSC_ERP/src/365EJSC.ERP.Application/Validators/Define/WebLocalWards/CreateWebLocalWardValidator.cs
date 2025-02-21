using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.Define;

namespace _365EJSC.ERP.Application.Validators.Define.WebLocalWards
{
    public class CreateWebLocalWardValidator : Validator<CreateWebLocalWardRequest>
    {
        public CreateWebLocalWardValidator()
        {
            WithValidator(MsgCode.ERR_WARD_INVALID);
            RuleFor(x => x.Name).NotNull()!.NotEmpty().MaxLength(WebLocalWardConstants.MAX_LENGTH_NAME);

            RuleFor(x => x.NameEn).NotNull()!.NotEmpty().MaxLength(WebLocalWardConstants.MAX_LENGTH_NAME_EN);

            RuleFor(x => x.FullName).NotNull()!.NotEmpty().MaxLength(WebLocalWardConstants.MAX_LENGTH_FULL_NAME);

            RuleFor(x => x.FullNameEn).NotNull()!.NotEmpty().MaxLength(WebLocalWardConstants.MAX_LENGTH_FULL_NAME_EN);

            RuleFor(x => x.Latitude).NotNull().GreaterThan(0);

            RuleFor(x => x.Longitude).NotNull().GreaterThan(0);

            RuleFor(x => x.DistrictId).NotNull().GreaterThan(0);
        }
    }
}
