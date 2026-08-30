namespace proxy_simulator.Constants
{
    public static class ServicesLogs
    {
        public static class Device
        {
            // ==================== Default Files / Configs ====================
            public const string DEFAULT_TELEMETRY_STREAM_FILE = "truck_decoded.txt";

            // ==================== Exceptions ====================
            public const string EXC_REMOVE_DEVICE_FAILED =
                "Failed to remove device '{0}' from one or more services.";

            public const string EXC_START_CHANNELS_FAILED =
                "Failed to start channels for device '{0}'.";

            public const string EXC_STOP_CHANNELS_FAILED =
                "Failed to stop channels for device '{0}'.";

            // ==================== Logs ====================
            public const string START_ALL_DEVICES_CHANNELS_ACTIVATED =
                "Received request to start all device channels.";

            public const string STOP_ALL_DEVICES_CHANNELS_ACTIVATED =
                "Received request to stop all device channels.";

            // ==================== Messages ====================
            public const string MSG_START_ALL_DEVICES_SUCCESS =
                "All device channels have been started.";

            public const string MSG_STOP_ALL_DEVICES_SUCCESS =
                "All device channels have been stopped.";

            public const string UNKNOWN_CHANNEL_TYPE =
                "Unknown channel type received: {ChannelType} (SimId: {SimId}) for device {DeviceName}";
        }

        public static class Multimedia
        {
            // ==================== Exceptions ====================

            // Upload File
            public const string EXC_UPLOAD_FILE_FAILED =
                "Failed to upload file '{0}'. StatusCode: {1}. ServerResponse: {2}";

            public const string EXC_UPLOAD_FILE_ERROR =
                "An error occurred while uploading file '{0}'.";

            public const string EXC_UPLOAD_FILE_INVALID_RESPONSE =
                "Invalid response received while uploading file '{0}'.";

            public const string EXC_UPLOAD_FILE_SERVER_REJECTED =
                "Server rejected upload for file '{0}'. Message: {1}";

            // Delete File
            public const string EXC_DELETE_FILE_FAILED =
                "Failed to delete file (SimId: {0}). StatusCode: {1}. ServerResponse: {2}";

            public const string EXC_DELETE_FILE_ERROR =
                "An error occurred while deleting file (SimId: {0}).";

            // Start Stream
            public const string EXC_START_STREAM_FAILED =
                "Failed to start stream (SimId: {0}). StatusCode: {1}. ServerResponse: {2}";

            public const string EXC_START_STREAM_ERROR =
                "An error occurred while starting stream (SimId: {0}).";

            // Stop Stream
            public const string EXC_STOP_STREAM_FAILED =
                "Failed to stop stream (SimId: {0}). StatusCode: {1}. ServerResponse: {2}";

            public const string EXC_STOP_STREAM_ERROR =
                "An error occurred while stopping stream (SimId: {0}).";

            // ==================== Logs ====================

            // Upload File
            public const string UPLOAD_FILE_SUCCESS =
                "[MultimediaService] File '{FileName}' uploaded successfully.";

            public const string UPLOAD_FILE_FAILED =
                "[MultimediaService] Failed to upload file '{FileName}'. [{StatusCode}]: {ServerError}";

            public const string UPLOAD_FILE_ERROR =
                "[MultimediaService] Error uploading file '{FileName}'.";

            // Delete File
            public const string DELETE_FILE_SUCCESS =
                "[MultimediaService] File deleted successfully (SimId: {SimId}).";

            public const string DELETE_FILE_FAILED =
                "[MultimediaService] Failed to delete file (SimId: {SimId}). StatusCode: {StatusCode}";

            public const string DELETE_FILE_ERROR =
                "[MultimediaService] Error deleting file (SimId: {SimId}).";

            // Start Stream
            public const string START_STREAM_SUCCESS =
                "[MultimediaService] Stream started successfully (SimId: {SimId}).";

            public const string START_STREAM_FAILED =
                "[MultimediaService] Failed to start stream (SimId: {SimId}). StatusCode: {StatusCode}, Error: {ServerError}";

            public const string START_STREAM_ERROR =
                "[MultimediaService] Error starting stream (SimId: {SimId}).";

            // Stop Stream
            public const string STOP_STREAM_SUCCESS =
                "[MultimediaService] Stream stopped successfully (SimId: {SimId}).";

            public const string STOP_STREAM_FAILED =
                "[MultimediaService] Failed to stop stream (SimId: {SimId}). StatusCode: {StatusCode}, Error: {ServerError}";

            public const string STOP_STREAM_ERROR =
                "[MultimediaService] Error stopping stream (SimId: {SimId}).";
        }

        public static class Telemetry
        {
            // ==================== Exceptions ====================
            public const string EXC_UPLOAD_KLV_FAILED =
                "Failed to upload KLV file '{0}'. StatusCode: {1}. ServerResponse: {2}";

            public const string EXC_DELETE_KLV_FAILED =
                "Failed to delete KLV stream for SimId '{0}'. StatusCode: {1}. ServerResponse: {2}";

            public const string EXC_START_STREAM_FAILED =
                "Failed to start telemetry stream for SimId '{0}'. StatusCode: {1}. ServerResponse: {2}";

            public const string EXC_STOP_STREAM_FAILED =
                "Failed to stop telemetry stream for SimId '{0}'. StatusCode: {1}. ServerResponse: {2}";

            public const string EXC_START_STREAM_INVALID_RESPONSE =
                "Invalid response received while starting telemetry stream for SimId '{0}'.";

            public const string EXC_STOP_STREAM_INVALID_RESPONSE =
                "Invalid response received while stopping telemetry stream for SimId '{0}'.";

            // ==================== Logs ====================

            // Upload
            public const string UPLOAD_KLV_SUCCESS =
                "[TelemetryService] KLV File '{FileName}' uploaded successfully.";

            public const string UPLOAD_KLV_FAILED =
                "[TelemetryService] Failed to upload KLV file '{FileName}'. StatusCode: {StatusCode}, Error: {Error}";

            public const string UPLOAD_KLV_ERROR =
                "[TelemetryService] Error uploading KLV file '{FileName}'.";

            // Delete
            public const string DELETE_KLV_SUCCESS =
                "[TelemetryService] KLV File deleted successfully (SimId: {SimId}).";

            public const string DELETE_KLV_FAILED =
                "[TelemetryService] Failed to delete KLV file (SimId: {SimId}). StatusCode: {StatusCode}, Error: {Error}";

            public const string DELETE_KLV_ERROR =
                "[TelemetryService] Error deleting KLV file (SimId: {SimId}).";

            // Start Stream
            public const string START_STREAM_SUCCESS =
                "[TelemetryService] Telemetry stream started for SimId {SimId}. Response: {Message}";

            public const string START_STREAM_FAILED =
                "[TelemetryService] Failed to start telemetry stream for SimId {SimId}. StatusCode: {StatusCode}, Error: {Error}";

            public const string START_STREAM_ERROR =
                "[TelemetryService] Error starting telemetry stream for SimId {SimId}.";

            // Stop Stream
            public const string STOP_STREAM_SUCCESS =
                "[TelemetryService] Telemetry stream stopped for SimId {SimId}. Response: {Message}";

            public const string STOP_STREAM_FAILED =
                "[TelemetryService] Failed to stop telemetry stream for SimId {SimId}. StatusCode: {StatusCode}, Error: {Error}";

            public const string STOP_STREAM_ERROR =
                "[TelemetryService] Error stopping telemetry stream for SimId {SimId}.";
        }

        public static class SQLite
        {
            public const string DB_INITIALIZED =
                "[SQLiteService] DB is ready!\n\n";

            public const string DB_DISPOSED =
                "[SQLiteService] DB is closed and disposed.";

            public const string CONNECTION_OPENED =
                "[SQLiteService] Connection established successfully to '{DataSource}'.";

            public const string DB_OPERATION_FAILED =
                "[SQLiteService] Database operation failed for query: {Query}";

            public static class ConfigExceptions
            {
                public const string PATH_NOT_IN_CONF =
                    "Connection string 'SQLiteDbPath' was not found in configuration.";
            }
        }

        public static class Lifecycle
        {
            public const string APP_STARTED_INIT_DB =
                "--> [Lifecycle] Application Started: initializing DB...";

            public const string APP_STOPPING_GRACEFUL =
                "--> [Lifecycle] Graceful Shutdown: Server is shutting down";

            public const string APP_STOPPED_CLEANUP =
                "--> [Lifecycle] Application Stopped: Server is completely closed, cleaning up...";

            public const string BANNER_API_LOGS =
                "\n\n\n==============================Application API logs:======================================";
        }
    }
}