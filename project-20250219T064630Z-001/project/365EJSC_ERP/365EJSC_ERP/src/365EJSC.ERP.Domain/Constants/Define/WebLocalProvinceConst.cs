using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Domain.Constants
{
    public class WebLocalProvinceConst
    {
        #region Database Defines

        public const string TABLE_NAME = "website_localization_province";
        public const string FIELD_ID = "province_id";
        public const string FIELD_NAME = "name";
        public const string FIELD_NAME_EN = "name_en";
        public const string FIELD_FULLNAME = "full_name";
        public const string FIELD_FULLNAME_EN = "full_name_en";
        public const string FIELD_LATITUDE = "latitude";
        public const string FIELD_LONGITUDE = "longitude";
        public const string FIELD_KEY_LOCALIZATION = "key_localization";

        #endregion

        #region Max length Defines

        public const int NAME_MAX_LENGTH = 32;
        public const int NAME_EN_MAX_LENGTH = 32;
        public const int FULLNAME_MAX_LENGTH = 64;
        public const int FULLNAME_EN_MAX_LENGTH = 64;
        public const int KEY_LOCALIZATION_MAX_LENGTH = 32;

        #endregion

        #region Message Defines

        public const string MSG_LOCALPROVINCE_ID_NOT_FOUND = $"{nameof(WebLocalProvince)} with this id was not found";

        #endregion
    }
}
