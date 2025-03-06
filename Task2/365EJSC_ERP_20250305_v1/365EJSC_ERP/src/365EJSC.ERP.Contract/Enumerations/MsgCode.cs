using System.Text.Json.Serialization;

namespace _365EJSC.ERP.Contract.Enumerations
{
    /// <summary>
    /// Enum to define error code, use for decompile into message for end user
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MsgCode
    {
        #region Local msg codes

        /// <summary>
        /// Local with id provided was not found
        /// </summary>
        ERR_KEY_LOCAL_NOT_FOUND,

        /// <summary>
		/// Local with id has exist in database
		/// </summary>
		ERR_KEY_LOCAL_EXISTED,

        /// <summary>
        /// Local request is invalid
        /// </summary>
        ERR_LOCAL_INVALID,

        #endregion

        #region Province msg codes

        /// <summary>
        /// Province with KeyLocalization was not found
        /// </summary>
        ERR_PROVINCE_ID_NOT_FOUND,

        /// <summary>
        /// Province request is invalid
        /// </summary>
        ERR_PROVINCE_INVALID,

        /// <summary>
        /// Cannot delete Province because it still contains Districts
        /// </summary>
        ERR_PROVINCE_HAS_DISTRICTS,

        #endregion

        #region District msg codes

        /// <summary>
        /// District with id provided was not found
        /// </summary>
        ERR_DISTRICT_ID_NOT_FOUND,

        /// <summary>
        /// District request is invalid
        /// </summary>
        ERR_DISTRICT_INVALID,

        /// <summary>
        /// Cannot delete District because it still contains Wards
        /// </summary>
        ERR_DISTRICT_HAS_WARDS,

        #endregion

        #region Ward msg codes

        /// <summary>
        /// Company with id provided was not found
        /// </summary>
        ERR_WARD_ID_NOT_FOUND,

        /// <summary>
        /// Company request is invalid
        /// </summary>
        ERR_WARD_INVALID,

        #endregion
        #region EmployeeRole msg codes

        /// <summary>
        /// EMPLOYEE with id provided was not found
        /// </summary>
        ERR_EMPLOYEE_ID_NOT_FOUND,

        /// <summary>
        /// EMPLOYEE request is invalid
        /// </summary>
        ERR_EMPLOYEE_INVALID,

        #endregion
        #region Marital msg codes
        ERR_MARITAL_ID_NOT_FOUND,
        ERR_MARITAL_INVALID,
        #endregion
        #region Company msg codes

        /// <summary>
        /// Company with id provided was not found
        /// </summary>
        ERR_COMPANY_ID_NOT_FOUND,

        /// <summary>
        /// Company request is invalid
        /// </summary>
        ERR_COMPANY_INVALID,

        #endregion

        #region CompanyDepartment msg codes

        /// <summary>
        /// CompanyDepartment with id provided was not found
        /// </summary>
        ERR_COMPANY_DEPARTMENT_ID_NOT_FOUND,

        /// <summary>
        /// CompanyDepartment request is invalid
        /// </summary>
        ERR_COMPANY_DEPARTMENT_INVALID,

        #endregion

        #region Department msg codes

        /// <summary>
        /// Department with id provided was not found
        /// </summary>
        ERR_DEPARTMENT_ID_NOT_FOUND,

        /// <summary>
        /// Department request is invalid
        /// </summary>
        ERR_DEPARTMENT_INVALID,

        /// <summary>
        /// Department request is duplicate id
        /// </summary>
        ERR_DEPARTMENT_DUPLICATE_ID,

        /// <summary>
        /// Department request is duplicate in database
        /// </summary>
        ERR_DEPARTMENT_DUPLICATE_IN_DATABASE,

        #endregion

        #region Position msg codes

        /// <summary>
        /// Position with id was not found
        /// </summary>
        ERR_POSITION_ID_NOT_FOUND,

        /// <summary>
        /// Position request is invalid
        /// </summary>
        ERR_POSITION_INVALID,

        #endregion

        #region Base msg codes

        /// <summary>
        /// Define error code for invalid email format
        /// </summary>
        /// <remarks>
        /// Correct example: abc@gmail.com
        /// </remarks>
        ERR_INVALID_EMAIL,

        /// <summary>
        /// Define error code for invalid phone format
        /// </summary>
        /// <remarks>
        /// Correct example: +84 123 123 1234
        /// </remarks>
        ERR_INVALID_PHONE,

        /// <summary>
        /// Define error for invalid key format
        /// </summary>
        /// <remarks>
        /// Correct example: ABC_123
        /// </remarks>
        ERR_INVALID_KEY,

        /// <summary>
        /// Define error code for internal server error
        /// </summary>
        ERR_INTERNAL_SERVER,

        /// <summary>
        /// Define error code for not found resources
        /// </summary>
        ERR_NOT_FOUND,

        /// <summary>
        /// Define error code for conflict between resources
        /// </summary>
        ERR_CONFLICT,

        /// <summary>
        /// Define error code for unexpected validation exception
        /// </summary>
        ERR_INVALID,

        /// <summary>
        /// Define error code for not found resources find by key
        /// </summary>
        ERR_NF_FIND_KEY,

        /// <summary>
        /// Define code for created message
        /// </summary>
        INF_CREATED,

        /// <summary>
        /// Define code for updated message
        /// </summary>
        INF_UPDATED,

        /// <summary>
        /// Define code for deleted message
        /// </summary>
        INF_DELETED,

        /// <summary>
        /// Define code for found resource
        /// </summary>
        INF_FOUND,

        #endregion
    }
}