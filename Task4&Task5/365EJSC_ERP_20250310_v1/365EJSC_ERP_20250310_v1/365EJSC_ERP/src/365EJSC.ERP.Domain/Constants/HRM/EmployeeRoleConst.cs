using _365EJSC.ERP.Domain.Entities.HRM;

namespace _365EJSC.ERP.Domain.Constants.HRM
{
    public class EmployeeRoleConst
    {
        #region Database defines
        public const string TABLE_NAME = "hrm_employee_role";
        public const string FIELD_ID = "emp_role_id";
        public const string FIELD_NAME = "er_name";
        public const string FIELD_CODE = "er_code";
        #endregion
        #region Max length defines
        public const int NAME_MAX_LENGTH = 256;
        public const int CODE_MAX_LENGTH = 32;
        #endregion
        #region Message defines
        public const string MSG_EMPLOYEE_ID_NOT_FOUND = $"{nameof(HrmEmployeeRole)} with this id was not found";
        #endregion
    }
}
