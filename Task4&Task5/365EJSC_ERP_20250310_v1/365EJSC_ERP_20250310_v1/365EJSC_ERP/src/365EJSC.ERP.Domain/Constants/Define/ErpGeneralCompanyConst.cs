using _365EJSC.ERP.Domain.Entities.Define;

namespace _365EJSC.ERP.Domain.Constants.Define
{
    public class ErpGeneralCompanyConst
    {
        #region Database defines

        public const string TABLE_NAME = "erp_general_company";
        public const string FIELD_ID = "company_id";
        public const string FIELD_COMPANT_PID = "company_pid";
        public const string FIELD_TAX_CODE = "com_tax_code";
        public const string FIELD_NAME = "com_name";
        public const string FIELD_IMAGE = "com_image";
        public const string FIELD_TEL = "com_tel";
        public const string FIELD_EMAIL = "com_email";
        public const string FIELD_WEBSITE = "com_website";
        public const string FIELD_FOUNDER = "com_founder";
        public const string FIELD_CEO = "com_ceo";
        public const string FIELD_CEO_IMAGE = "com_ceo_image";
        public const string FIELD_CEO_EMAIL = "com_ceo_email";
        public const string FIELD_CEO_TEL = "com_ceo_tel";
        public const string FIELD_LICENSE = "com_license";
        public const string FIELD_COUNTRY_ID = "country_id";
        public const string FIELD_WARD_ID = "ward_id";
        public const string FIELD_IS_ACTIVED = "is_actived";

        #endregion

        #region Max length defines

        public const int TAX_CODE_MAX_LENGTH = 64;
        public const int NAME_MAX_LENGTH = 128;
        public const int IMAGE_MAX_LENGTH = 128;
        public const int TEL_MAX_LENGTH = 64;
        public const int EMAIL_MAX_LENGTH = 64;
        public const int WEBSITE_MAX_LENGTH = 64;
        public const int FOUNDER_MAX_LENGTH = 64;
        public const int CEO_MAX_LENGTH = 64;
        public const int CEO_IMAGE_MAX_LENGTH = 128;
        public const int CEO_EMAIL_MAX_LENGTH = 64;
        public const int CEO_TEL_MAX_LENGTH = 64;
        public const int LICENSE_MAX_LENGTH = 64;
        public const int COUNTRY_ID_MAX_LENGTH = 32;

        #endregion

        #region Message defines

        public const string MSG_COMPANY_ID_NOT_FOUND = $"{nameof(ErpGeneralCompany)} with this id was not found";
        public const string MSG_COUNTRY_ID_NOT_FOUND = $"Country with this id was not found";
        public const string MSG_DUPLICATE_ID = "Duplicate Department found with Id {0}";
        public const string MSG_DEPARTMENT_ID_NOT_FOUND = "Department not found with Id {0}";
        public const string MSG_DUPLICATE_DEPARTMENT_IN_DATABASE = "A room ban already exists within the company with Id {0}";

        #endregion
    }
}