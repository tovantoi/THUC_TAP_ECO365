using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Domain.Constants.Define
{
    public class WebLocalWardConst
    {
        #region Database defines
        public const string TABLE_NAME = "website_localization_ward";

        public const string FIELD_WARD_ID = "ward_id";
        public const string FIELD_NAME = "name";
        public const string FIELD_NAME_EN = "name_en";
        public const string FIELD_FULL_NAME = "full_name";
        public const string FIELD_FULL_NAME_EN = "full_name_en";
        public const string FIELD_LATITUDE = "latitude";
        public const string FIELD_LONGITUDE = "longitude";
        public const string FIELD_DISTRICT_ID = "district_id";
        #endregion
        #region Max length defines
        public const int MAX_LENGTH_NAME = 64;
        public const int MAX_LENGTH_NAME_EN = 64;
        public const int MAX_LENGTH_FULL_NAME = 96;
        public const int MAX_LENGTH_FULL_NAME_EN = 96;
        #endregion
        #region Message defines
        public const string MSG_WARD_ID_NOT_FOUND = $"{nameof(WebLocalWard)} with this id was not found";
        public const string ALREADY_EXISTS = "{0} already exists.";
        #endregion
    }
}
