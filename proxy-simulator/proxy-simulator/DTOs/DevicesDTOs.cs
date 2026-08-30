using Microsoft.AspNetCore.Mvc;

namespace proxy_simulator.DTOs
{
    public class DevicesDTOs
    {
        public sealed class AddDevice
        {
            [FromForm(Name = "multimediaFile")]
            public IFormFile MultimediaFile { get; init; } = null!;

            [FromForm(Name = "telemetryFile")]
            public IFormFile TelemetryFile { get; init; } = null!;
        }
        public sealed class RemoveDevice
        {
            public required string deviceName { get; init; }
        }
        public sealed class StartDeviceChanneles
        {
            public required string deviceName { get; init; }
        }
        public sealed class StopDeviceChanneles
        {
            public required string deviceName { get; init; }
        }
    }
}
