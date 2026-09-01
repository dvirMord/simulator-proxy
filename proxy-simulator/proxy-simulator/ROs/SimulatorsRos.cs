using System.Text.Json.Serialization;
using static proxy_simulator.DTOs.MultimediaApiDTO;
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

            public class StartStreamResponse
            {
                [JsonPropertyName("success")]
                public bool Success { get; set; }

                [JsonPropertyName("message")]
                public string Message { get; set; } = string.Empty;

                [JsonPropertyName("rtspStream")]
                public string RtspStream { get; set; } = string.Empty;
            }

            public class GetAllActiveStreamResponse
            {
                [JsonPropertyName("success")]
                public bool Success { get; set; }

                [JsonPropertyName("streams")]
                public IEnumerable<ChannelDTO> Streams { get; set; } = new List<ChannelDTO>();
            }
        }

        public class Telemetry
        {
            public sealed class UploadFileResponse
            {
                [JsonPropertyName("success")]
                public bool Success { get; init; }

                [JsonPropertyName("message")]
                public string Message { get; init; } = string.Empty;

                [JsonPropertyName("decodedId")]
                public int DecodedId { get; init; }
            }
        }
    }
}
