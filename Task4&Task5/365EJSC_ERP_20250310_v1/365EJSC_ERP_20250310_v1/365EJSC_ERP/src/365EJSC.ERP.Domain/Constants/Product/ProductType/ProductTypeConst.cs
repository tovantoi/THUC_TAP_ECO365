using _365EJSC.ERP.Domain.Entities.Product.ProductType;

namespace _365EJSC.ERP.Domain.Constants.Product.ProductType
{
    public class ProductTypeConst
    {
        #region Database defines
        public const string TABLE_NAME = "product_product_type";
        public const string FIELD_ID = "type_id";
        public const string FIELD_NAME = "typ_name";
        #endregion
        #region Max length defines
        public const int NAME_MAX_LENGTH = 128;
        #endregion
        #region Message defines
        public const string MSG_PRODUCT_TYPE_ID_NOT_FOUND = $"{nameof(PdProductType)} with this id was not found";
        #endregion
    }
}
