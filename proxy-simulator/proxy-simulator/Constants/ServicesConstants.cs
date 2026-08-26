using Microsoft.VisualBasic;

namespace proxy_simulator.Constants
{
    public static class ServicesConstants
    {
        public static class SQlite
        {
            public static class Settings
            {
                public const string APP_SETTING_KEY = "SQLiteDbPath";
            }
        }

        public static class Program 
        {
            public static class Swagger
            {
                public const string SWAGGER_URL = "/swagger/v1/swagger.json";
                public const string SWAGGER_NAME = "Proxy Simulator API v1";
            }
        }
    }
}
