using _365EJSC.ERP.Domain.Entities.HRM;

namespace _365EJSC.ERP.Domain.Constants.HRM
{
    public class DefineSalaryStructureConst
    {
        #region Database defines

        public const string TABLE_NAME = "hrm_define_salary_structure";
        public const string FIELD_ID = "sal_str_id";
        public const string FIELD_CODE = "ss_code";
        public const string FIELD_NAME = "ss_name";

        #endregion

        #region Max length defines

        public const int CODE_MAX_LENGTH = 32;
        public const int NAME_MAX_LENGTH = 256;

        #endregion

        #region Message defines

        public const string MSG_SALARY_STRUCTURE_ID_NOT_FOUND = $"{nameof(DefineSalaryStructure)} with this id was not found";

        #endregion
    }
}