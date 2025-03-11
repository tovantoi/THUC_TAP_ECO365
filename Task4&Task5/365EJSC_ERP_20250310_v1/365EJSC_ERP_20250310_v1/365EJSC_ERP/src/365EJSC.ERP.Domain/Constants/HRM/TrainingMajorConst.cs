using _365EJSC.ERP.Domain.Entities.HRM;

namespace _365EJSC.ERP.Domain.Constants.HRM
{
    public class TrainingMajorConst 
    {
        #region Database defines

        public const string TABLE_NAME = "hrm_training_major";
        public const string FIELD_ID = "tra_maj_id";
        public const string FIELD_NAME = "tm_name";

        #endregion

        #region Max length defines

        public const int MAX_LENGTH_NAME = 256;

        #endregion

        #region Message defines
        public const string MSG_TRAININGMAJOR_ID_NOT_FOUND = $"{nameof(TrainingMajor)} with this id was not found";
        #endregion
    }
}
