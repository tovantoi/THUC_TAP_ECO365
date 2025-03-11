using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Domain.Constants.Define
{
    public class WebLocalConst
    {
        #region Database defines 
        public const string TABLE_NAME = "website_localization";
        public const string FIELD_KEY_LOCALIZATION = "key_localization";
        public const string FIELD_LOCALIZATION = "localization";
        public const string FIELD_IS_ACTIVED = "is_actived";
        #endregion

        #region Max length defines

        public const int KEY_LOCALIZATION_MAX_LENGTH = 32;
        public const int LOCALIZATION_MAX_LENGTH = 32;

        #endregion

        #region Message defines

        public const string MSG_KEY_LOCALIZATION_NOT_FOUND = $"{nameof(WebLocals)} with this key localization was not found";
        public const string MSG_KEY_LOCALIZATION_EXISTED = $"{nameof(WebLocals)} with this key localization has existed in database";

        #endregion
    }
}