using proxy_simulator.DTOs;

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
            public static class ChannelType
            {
                public const string Multimedia = "Multimedia";
                public const string Telemetry = "Telemetry";
            }

            public static class Queries
            {
                public const string GET_ALL_DEVICES = "SELECT name FROM devices;";
                public const string INIT_DB = @"
                    PRAGMA journal_mode = WAL;
                    PRAGMA foreign_keys = ON;

                    CREATE TABLE IF NOT EXISTS Devices (
                        Name TEXT PRIMARY KEY
                    );

                    CREATE TABLE IF NOT EXISTS Channels (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Type TEXT NOT NULL,
                        SimId INTEGER NOT NULL,
                        DeviceName TEXT NOT NULL,

                        CONSTRAINT FK_Channels_Devices FOREIGN KEY (DeviceName) 
                            REFERENCES Devices(Name) ON DELETE CASCADE
                    );

                    CREATE INDEX IF NOT EXISTS IX_Channels_DeviceName ON Channels (DeviceName);";

                public const string INSERT_DEVICE = @"
                    INSERT INTO Devices (Name) 
                    VALUES (@DeviceName);";

                public const string INSERT_CHANNEL = @"
                    INSERT INTO Channels (Type, SimId, DeviceName) 
                    VALUES (@Type, @SimId, @DeviceName);";
            }
        }

        public static class Telemetry
        {
            public const string FILE_FORM = "application/octet-stream";
        }

        public static class ExceptionFilter
        {
            public const string UNKNOWED_EXCEPTION = "Unhandled exception. Path: {Path}";
        }

        public static class Program
        {
            public const string FILE_FORM = "application/octet-stream";
            public static class Swagger
            {
                public const string SWAGGER_URL = "/swagger/v1/swagger.json";
                public const string SWAGGER_NAME = "Proxy Simulator API v1";
            }
        }
        public static class Multemedia
        {
            public const string HTTP_FILE_HEADER_NAME = "file";
        }
    }
}