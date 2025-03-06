using _365EJSC.ERP.Domain.Entities.Define;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _365EJSC.ERP.Domain.Constants.Define
{
    public class GeneralDepartmentConst
    {
        #region Database Defines

        public const string TABLE_NAME = "erp_general_department";
        public const string FIELD_ID = "department_id";
        public const string FIELD_DE_CODE = "de_code";
        public const string FIELD_DE_NAME = "de_name";
        public const string FIELD_IS_ACTIVED = "is_actived";

        #endregion

        #region Max Length Defines

        public const int DE_CODE_MAX_LENGTH = 32;
        public const int DE_NAME_MAX_LENGTH = 128;

        #endregion

        #region Message Defines

        public const string MSG_DEPARTMENT_ID_NOT_FOUND = $"{nameof(GeneralDepartment)} with this id was not found";
        public const string INACTIVE_DEPARTMENT_REQUIRED_MSG = $"{nameof(GeneralDepartment)} verification can only be deleted if it is inactive.";
        #endregion

    }
}
