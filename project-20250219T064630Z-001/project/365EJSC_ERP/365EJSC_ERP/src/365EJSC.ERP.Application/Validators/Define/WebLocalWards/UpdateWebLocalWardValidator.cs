using _365EJSC.ERP.Application.Requests.Define.WebLocalWards;
using _365EJSC.ERP.Contract.DependencyInjection.Extensions;
using _365EJSC.ERP.Contract.Enumerations;
using _365EJSC.ERP.Contract.Validators;
using _365EJSC.ERP.Domain.Constants.Define;

namespace _365EJSC.ERP.Application.Validators.Define.WebLocalWards
{ 
    public class UpdateWebLocalWardValidator : Validator<UpdateWebLocalWardRequest>
    {
        public UpdateWebLocalWardValidator()
        {
            WithValidator(MsgCode.ERR_WARD_INVALID);
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaxLength(WebLocalWardConstants.MAX_LENGTH_NAME);

            RuleFor(x => x.NameEn).NotEmpty().MaxLength(WebLocalWardConstants.MAX_LENGTH_NAME_EN);

            RuleFor(x => x.FullName).NotEmpty().MaxLength(WebLocalWardConstants.MAX_LENGTH_FULL_NAME);

            RuleFor(x => x.FullNameEn).NotEmpty().MaxLength(WebLocalWardConstants.MAX_LENGTH_FULL_NAME_EN);

            RuleFor(x => x.Latitude).GreaterThan(0);

            RuleFor(x => x.Longitude).GreaterThan(0);

            RuleFor(x => x.DistrictId).GreaterThan(0);
        }
    }
}
