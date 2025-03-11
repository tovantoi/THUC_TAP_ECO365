using _365EJSC.ERP.Domain.Entities.Define;
using _365EJSC.ERP.Domain.Entities.HRM;

namespace _365EJSC.ERP.Domain.Constants.HRM
{
    public class ContractTypeConst
    {
        #region Database Defines

        public const string TABLE_NAME = "hrm_define_contract_type";
        public const string FIELD_ID = "con_typ_id";
        public const string FIELD_CODE = "ct_code";
        public const string FIELD_NAME = "ct_name";

        #endregion

        #region Max length Defines

        public const int CODE_MAX_LENGTH = 32;
        public const int NAME_MAX_LENGTH = 256;

        #endregion

        #region Message Defines

        public const string MSG_CONTRACT_TYPE_ID_NOT_FOUND = $"{nameof(DefineContractType)} with this id was not found";

        #endregion
    }
}
