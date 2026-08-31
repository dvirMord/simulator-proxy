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
            [JsonPropertyName("simId")]
            public int SimId { get; init; }
        }
        public class StartStreamDTO
        {
            [JsonPropertyName("simId")]
            public int SimId { get; set; }
        }

        public class StopStreamDTO
        {
            [JsonPropertyName("simId")]
            public int SimId { get; set; }
        }
    }
}
