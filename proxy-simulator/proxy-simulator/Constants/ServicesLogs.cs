namespace proxy_simulator.Constants
{
    public static class ServicesLogs
    {
        public static class Multimedia
        {
            // Upload File
            public const string UPLOAD_FILE_SUCCESS = "[MultimediaService] File '{FileName}' uploaded successfully.";
            public const string UPLOAD_FILE_FAILED = "[MultimediaService] Failed to upload file '{FileName}'. StatusCode: {StatusCode}";
            public const string UPLOAD_FILE_ERROR = "[MultimediaService] Error uploading file '{FileName}'.";

            // Delete File
            public const string DELETE_FILE_SUCCESS = "[MultimediaService] File '{FileName}' deleted successfully.";
            public const string DELETE_FILE_FAILED = "[MultimediaService] Failed to delete file '{FileName}'. StatusCode: {StatusCode}";
            public const string DELETE_FILE_ERROR = "[MultimediaService] Error deleting file '{FileName}'.";

            // Start Stream
            public const string START_STREAM_SUCCESS = "[MultimediaService] Stream started successfully for file '{FileName}' (SourceFileId: {SourceFileId}).";
            public const string START_STREAM_FAILED = "[MultimediaService] Failed to start stream for file '{FileName}'. StatusCode: {StatusCode}, Error from server: {ServerError}";
            public const string START_STREAM_ERROR = "[MultimediaService] Error starting stream for file '{FileName}'.";

            // Stop Stream
            public const string STOP_STREAM_SUCCESS = "[MultimediaService] Stream '{StreamName}' stopped successfully.";
            public const string STOP_STREAM_FAILED = "[MultimediaService] Failed to stop stream '{StreamName}'. StatusCode: {StatusCode}, Error from server: {ServerError}";
            public const string STOP_STREAM_ERROR = "[MultimediaService] Error stopping stream '{StreamName}'.";
        }
        public static class SQLite
        {
            public const string DB_INITIALIZED = "[SQLiteService] DB is ready!\n\n";
            public const string DB_DISPOSED = "[SQLiteService] DB is closed and disposed.";
            public const string CONNECTION_OPENED = "[SQLiteService] Connection established successfully to '{DataSource}'.";
            public const string DB_OPERATION_FAILED = "[SQLiteService] Database operation failed for query: {Query}";
            public static class ConfigExceptions
            {
                public const string PATH_NOT_IN_CONF = "Connection string 'SQLiteDbPath' was not found in configuration.";
            }
        }

        public static class Lifecycle
        {
            public const string APP_STARTED_INIT_DB = "--> [Lifecycle] Application Started: initializing DB...";
            public const string APP_STOPPING_GRACEFUL = "--> [Lifecycle] Graceful Shutdown: Server is shutting down";
            public const string APP_STOPPED_CLEANUP = "--> [Lifecycle] Application Stopped: Server is completely closed, cleaning up...";
            public const string BANNER_API_LOGS = "\n\n\n==============================Application API logs:======================================";
        }
    }
}