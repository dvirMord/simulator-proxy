namespace proxy_simulator.DTOs
{
    public class DevicesDTOs
    {
        public sealed class AddDevice
        {
            public required string deviceName { get; init; }
        }
        public sealed class RemoveDevice
        {
            public required string deviceName { get; init; }
        }
        public sealed class UpdateDevice
        {
            public required string deviceName { get; init; }
        }
    }
}
