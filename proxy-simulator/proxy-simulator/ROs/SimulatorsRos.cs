using System.Text.Json.Serialization;

namespace proxy_simulator.ROs
{
    public static class SimulatorsRos
    {
        public class Multimedia
        {
            public class UploadFileResponse
            {
                [JsonPropertyName("success")]
                public bool Success { get; set; }

                [JsonPropertyName("message")]
                public string Message { get; set; } = string.Empty;

                [JsonPropertyName("idInDb")]
                public int IdInDb { get; set; }
            }
        }

        public class Telemetry
        {

        }
    }
}
