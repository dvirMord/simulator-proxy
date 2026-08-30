namespace proxy_simulator.Constants
{
    public static class ControllersLogs
    {
        public static class Device
        {
            // Devices
            public const string ADD_DEVICE_ACTIVATED = "[Controller->Device] AddDevice endpoint activated for {TelemetryFileName} and {MultimediaFileName}.";
            public const string REMOVE_DEVICE_ACTIVATED = "[Controller->Device] RemoveDevice endpoint activated for {DeviceName}.";
            public const string GET_ALL_DEVICES_ACTIVATED = "[Controller->Device] GetAllDevices endpoint activated.";

            // Channels
            public const string START_DEVICE_CHANNELS_ACTIVATED = "[Controller->Device] StartDeviceChanneles endpoint activated for {DeviceName}.";
            public const string STOP_DEVICE_CHANNELS_ACTIVATED = "[Controller->Device] StopDeviceChanneles endpoint activated for {DeviceName}.";
            public const string START_ALL_DEVICES_CHANNELS_ACTIVATED = "[Controller->Device] StartAllDevicesChanneles endpoint activated.";
            public const string STOP_ALL_DEVICES_CHANNELS_ACTIVATED = "[Controller->Device] StopAllDevicesChanneles endpoint activated.";
        }
    }
}