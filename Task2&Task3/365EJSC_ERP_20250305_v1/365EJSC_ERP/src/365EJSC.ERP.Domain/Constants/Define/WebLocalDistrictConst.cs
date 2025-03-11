namespace _365EJSC.ERP.Domain.Constants.Define
{
    public class WebLocalDistrictConst
    {
        #region Database Defines

        public const string TABLE_NAME = "website_localization_district";
        public const string FIELD_ID = "district_id";
        public const string FIELD_NAME = "name";
        public const string FIELD_NAME_EN = "name_en";
        public const string FIELD_FULL_NAME = "full_name";
        public const string FIELD_FULL_NAME_EN = "full_name_en";
        public const string FIELD_LATITUDE = "latitude";
        public const string FIELD_LONGITUDE = "longitude";
        public const string FIELD_PROVINCE_ID = "province_id";

        #endregion

        #region Max length Defines

        public const int NAME_MAX_LENGTH = 64;
        public const int NAME_EN_MAX_LENGTH = 64;
        public const int FULL_NAME_MAX_LENGTH = 96;
        public const int FULL_NAME_EN_MAX_LENGTH = 96;

        #endregion

        #region Message Defines

        public const string MSG_DISTRICT_ID_NOT_FOUND = $"{nameof(Entities.Define.WebLocalDistrict)} with this id was not found";
        public const string MSG_DISTRICT_PROVINCE_EXIST = $"{nameof(Entities.Define.WebLocalDistrict)} has exist";
        public const string MSG_CANNOT_DELETE_DISTRICT_WITH_WARDS = $"{nameof(Entities.Define.WebLocalDistrict)} cannot be deleted because it has associated wards.";

        #endregion
    }
}
