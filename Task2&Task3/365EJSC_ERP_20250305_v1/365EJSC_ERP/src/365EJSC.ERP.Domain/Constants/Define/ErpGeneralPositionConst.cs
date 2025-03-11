using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Domain.Constants.Define
{
    public class ErpGeneralPositionConst
    {
        #region Database Defines

        public const string TABLE_NAME = "erp_general_position";
        public const string FIELD_ID = "position_id";
        public const string FIELD_CODE = "pos_code";
        public const string FIELD_NAME = "pos_name";

        #endregion

        #region Max length Defines

        public const int CODE_MAX_LENGTH = 32;
        public const int NAME_MAX_LENGTH = 256;

        #endregion

        #region Message Defines

        public const string MSG_GENERAL_POSITION_ID_NOT_FOUND = $"{nameof(ErpGeneralPosition)} with this id was not found";

        #endregion
    }
}
