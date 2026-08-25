namespace proxy_simulator.DTOs
{
    public class DevicesDTOs
    {
        public sealed class AddDevice
        {
            public required IFormFile multimediaFile { get; init; }
            public required IFormFile telemetryFile { get; init; }
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
