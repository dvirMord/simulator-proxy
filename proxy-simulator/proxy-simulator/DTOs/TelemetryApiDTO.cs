using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace proxy_simulator.DTOs
{
    public static class TelemetryApiDTO
    {
        public class DeleteFileDTO
        {
            [JsonPropertyName("simId")]
            [Required(ErrorMessage = "SimId is required.")]
            [Range(1, int.MaxValue, ErrorMessage = "SimId must be a positive integer.")]
            public int SimId { get; set; }
        }

        public class StartStreamDTO
        {
            [JsonPropertyName("simId")]
            [Required(ErrorMessage = "SimId is required.")]
            [Range(1, int.MaxValue, ErrorMessage = "SimId must be a positive integer.")]
            public int SimId { get; set; }
        }

        public class StopStreamDTO
        {
            [JsonPropertyName("simId")]
            [Required(ErrorMessage = "SimId is required.")]
            [Range(1, int.MaxValue, ErrorMessage = "SimId must be a positive integer.")]
            public int SimId { get; set; }
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