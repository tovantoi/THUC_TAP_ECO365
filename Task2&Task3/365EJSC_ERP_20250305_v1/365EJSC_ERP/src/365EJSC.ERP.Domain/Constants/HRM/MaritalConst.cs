using _365EJSC.ERP.Domain.Entities.HRM;

namespace _365EJSC.ERP.Domain.Constants.HRM
{
    public class MaritalConst
    {
        #region Database defines
        public const string TABLE_NAME = "hrm_marital";
        public const string FIELD_ID = "marital_id";
        public const string FIELD_NAME = "mar_name";
        #endregion
        #region Max length defines
        public const int NAME_MAX_LENGTH = 256;
        #endregion
        #region Message defines
        public const string MSG_MARITAL_ID_NOT_FOUND = $"{nameof(HrmMarital)} with this id was not found";
        #endregion
    }
}
