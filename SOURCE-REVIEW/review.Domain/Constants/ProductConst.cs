using review.Domain.Entities;

namespace review.Domain.Constants
{
    public class ProductConst
    {
        #region Database defines

        public const string TABLE_NAME = "Products";
        public const string FIELD_NAME = "Name";
        public const string FIELD_DESCRIPTION = "Description";
        #endregion

        #region Max length defines

        public const int NAME_MAX_LENGTH = 255;
        public const int DESCRIPTION_MAX_LENGTH = 512;
        public const int IMAGE_URL_MAX_LENGTH = 255;

        #endregion

        #region Message defines

        public const string MSG_PRODUCT_ID_NOT_FOUND = $"{nameof(Product)} with this ID was not found";

        #endregion
    }
}