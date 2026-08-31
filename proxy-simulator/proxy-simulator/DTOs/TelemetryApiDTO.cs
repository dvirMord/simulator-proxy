using proxy_simulator.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace proxy_simulator.DTOs
{
    public static class TelemetryApiDTO
    {
        public class DeleteFileDTO
        {
            [JsonPropertyName("simId")]
            [Required(ErrorMessage = ServicesConstants.Telemetry.SIM_MISSING_ERROR)]
            [Range(1, int.MaxValue, ErrorMessage = ServicesConstants.Telemetry.SIM_POSITIVE_ERROR)]
            public int SimId { get; set; }
        }

        public class StartStreamDTO
        {
            [JsonPropertyName("simId")]
            [Required(ErrorMessage = ServicesConstants.Telemetry.SIM_MISSING_ERROR)]
            [Range(1, int.MaxValue, ErrorMessage = ServicesConstants.Telemetry.SIM_POSITIVE_ERROR)]
            public int SimId { get; set; }
        }

        public class StopStreamDTO
        {
            [JsonPropertyName("simId")]
            [Required(ErrorMessage = ServicesConstants.Telemetry.SIM_MISSING_ERROR)]
            [Range(1, int.MaxValue, ErrorMessage = ServicesConstants.Telemetry.SIM_POSITIVE_ERROR)]
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