namespace _365Architect.Demo.Contract.Constants
{
    /// <summary>
    /// Contain all constant for application
    /// </summary>
    public static class Const
    {
        #region Connection

        public const string CONN_CONFIG_SQL = "DbSqlServer";
        public const string CONN_CONFIG_MONGO = "DbMongo:ConnectionString";
        public const string DB_MONGO = "DbMongo:Database";

        #endregion

        #region rabbitMQ

        public const string BROKER_CONFIG = "MessageBroker";
        public const string BROKER_HOST = "Host";
        public const string BROKER_USERNAME = "Username";
        public const string BROKER_PASSWORD = "Password";

        #endregion
    }
}