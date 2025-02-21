using _365Architect.Demo.Domain.Entities;

namespace _365Architect.Demo.Domain.Constants
{
    public class SampleConst
    {
        #region Database defines

        public const string TABLE_NAME = "Sample";
        public const string FIELD_DESCRIPTION = "Description";
        public const string FIELD_TITLE = "Title";
        public const string FIELD_CREATED_AT = "CreatedAt";
        public const string FIELD_UPDATED_AT = "UpdatedAt";
        public const string FIELD_DUE_DATE = "DueDate";

        #endregion

        #region Max length defines

        public const int TITLE_MAX_LENGTH = 128;
        public const int DESCRIPTION_MAX_LENGTH = 128;

        #endregion

        #region Message defines

        public const string MSG_SAMPLE_ID_NOT_FOUND = $"{nameof(Sample)} with this id was not found";

        #endregion
    }
}