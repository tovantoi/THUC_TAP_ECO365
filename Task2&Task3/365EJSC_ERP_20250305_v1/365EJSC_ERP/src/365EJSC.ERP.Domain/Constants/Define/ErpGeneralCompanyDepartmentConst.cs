using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Domain.Constants.Define
{
    public class ErpGeneralCompanyDepartmentConst
    {
        #region Database defines

        public const string TABLE_NAME = "erp_general_company_department";
        public const string FIELD_ID = "cd_id";
        public const string FIELD_COMPANY_ID = "company_id";
        public const string FIELD_DEPARTMENT_ID = "department_id";

        #endregion

        #region Max length defines

        #endregion

        #region Message defines

        public const string MSG_COMPANY_DEPARTMENT_ID_NOT_FOUND = $"{nameof(ErpGeneralCompanyDepartment)} with this id was not found";

        #endregion
    }
}