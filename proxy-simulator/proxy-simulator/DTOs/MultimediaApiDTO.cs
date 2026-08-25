using System.Text.Json.Serialization;

namespace proxy_simulator.DTOs
{
    public class MultimediaApiDTO
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum StreamType
        {
            Video = 0,
            Audio = 1
        }

        public class DeleteFileDTO
        {
            [JsonPropertyName("fileName")]
            public string FileName { get; init; } = string.Empty;
        }
        public class StartStreamDTO
        {
            [JsonPropertyName("fileName")]
            public string FileName { get; set; } = string.Empty;

            [JsonPropertyName("sourceFileId")]
            public int SourceFileId { get; set; }

            [JsonPropertyName("type")]
            public StreamType Type { get; set; } = StreamType.Video;
        }
        public class StopStreamDTO
        {
            [JsonPropertyName("streamName")]
            public string StreamName { get; init; } = string.Empty;
        }
    }
}
