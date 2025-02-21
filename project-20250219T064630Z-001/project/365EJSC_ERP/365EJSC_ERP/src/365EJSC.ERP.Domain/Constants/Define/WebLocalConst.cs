
using _365EJSC.ERP.Domain.Entities;

namespace _365EJSC.ERP.Domain.Constants
{
	public class WebLocalConst
	{
		#region Database defines 
		public const string TABLE_NAME = "website_localization";
		public const string FIELD_KEY_LOCALIZATION = "key_localization";
		public const string FIELD_LOCALIZATION = "localization";
		public const string FIELD_IS_ACTIVED = "is_actived";
		#endregion

		#region Max length defines

		public const int KEY_LOCALIZATION_MAX_LENGTH = 32;
		public const int LOCALIZATION_MAX_LENGTH = 32;

		#endregion

		#region Message defines

		public const string MSG_KEY_LOCALIZATION_NOT_FOUND = $"{nameof(WebLocals)} with this key localization was not found";
		public const string MSG_LOCALIZATION_NOT_FOUND = $"{nameof(WebLocals)} with this localization was not found";
		public const string MSG_IS_ACTIVED_INVALID = "The value for is_actived is invalid. It must be either True or False.";

		#endregion



	}
}
