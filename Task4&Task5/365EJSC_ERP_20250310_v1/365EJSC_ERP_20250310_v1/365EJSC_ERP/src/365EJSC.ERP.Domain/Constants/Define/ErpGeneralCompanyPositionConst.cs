using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Domain.Constants.Define
{
    public class ErpGeneralCompanyPositionConst
    {
        #region Database defines

        public const string TABLE_NAME = "erp_general_company_position";
        public const string FIELD_ID = "cp_id";
        public const string FIELD_COMPANY_ID = "company_id";
        public const string FIELD_POSITION_ID = "position_id";

        #endregion

        #region Max length defines

        #endregion

        #region Message defines

        public const string MSG_COMPANY_POSITION_ID_NOT_FOUND = $"{nameof(ErpGeneralCompanyPosition)} with this id was not found";
        public const string MSG_DUPLICATE_ID = "Duplicate POSITION found with Id {0}";
        public const string MSG_POSITION_ID_NOT_FOUND = "POSITION not found with Id {0}";
        public const string MSG_DUPLICATE_POSITION_IN_DATABASE = "A room ban already exists within the company with Id {0}";

        #endregion
    }
}