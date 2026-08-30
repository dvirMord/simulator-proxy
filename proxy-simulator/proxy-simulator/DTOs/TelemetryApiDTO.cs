using System.Text.Json.Serialization;

namespace proxy_simulator.DTOs
{
    public static class TelemetryApiDTO
    {
        public class DeleteFileDTO
        {
            [JsonPropertyName("file_name")]
            public string FileName { get; set; } = string.Empty;
        }

        public class StartStreamDTO
        {
            [JsonPropertyName("file_name")]
            public string FileName { get; set; } = string.Empty;
        }

        public class StopStreamDTO
        {
            [JsonPropertyName("file_name")]
            public string FileName { get; set; } = string.Empty;
        }

        public class StreamResponseDTO
        {
            [JsonPropertyName("success")]
            public bool Success { get; set; }

            [JsonPropertyName("message")]
            public string Message { get; set; } = string.Empty;
        }
    }
}