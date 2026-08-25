namespace proxy_simulator.Constants
{
    public static class DBConstants
    {
        public static class ConfigExceptions
        {
            public const string PATH_NOT_IN_CONF = "Connection string 'SQLiteDbPath' was not found in configuration.";
        }
        public static class Settings
        {
            public const string APP_SETTING_KEY = "SQLiteDbPath";
            public const string DEFUALT_PATH = "simulator-proxy.db";
        }
        public static class Logs
        {
            public static string SUCCESSFULLY_READY_LOG = "[SQLiteService] DB is ready!";
            public static string SUCCESSFULLY_CLEAR_N_DISPOSE_LOG_ = "SQLiteService] DB is closed and disposed.";
        }
    }
}
