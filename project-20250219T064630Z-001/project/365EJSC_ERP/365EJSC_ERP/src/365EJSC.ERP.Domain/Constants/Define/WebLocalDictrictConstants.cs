using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Domain.Constants.Define;

public class WebLocalDictrictConstants
{
    public const string TABLE_NAME = "website_localization_district";

    public const string FIELD_ID_DICTRICT = "district_id";
    public const string FIELD_NAME = "name";

    public const string MSG_DICTRICT_ID_NOT_FOUND = $"{nameof(WebLocalDictrict)} with this id was not found";
    public const string ALREADY_EXISTS = "{0} already exists.";
}
