using _365EJSC.ERP.Domain.Entities.HRM;


namespace _365EJSC.ERP.Domain.Constants.HRM
{
	public class DegreeConst
	{
		#region Database defines 
		public const string TABLE_NAME = "hrm_degree";
		public const string FIELD_DEGREE_ID = "degree_id";
		public const string FIELD_DEG_NAME = "deg_name";
		#endregion

		#region Max length defines

		public const int DEG_NAME_MAX_LENGTH = 256;

		#endregion

		#region Message defines

		public const string MSG_DEGREE_ID_NOT_FOUND = $"{nameof(Degree)} with this degree id was not found";
		public const string MSG_DEGREE_ID_EXISTED = $"{nameof(Degree)} with this degree id has existed in database";

		#endregion
	}
}
