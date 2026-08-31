using proxy_simulator.DTOs;
using static proxy_simulator.DTOs.DevicesDTOs;

namespace proxy_simulator.Interfaces
{
    public interface IDeviceService
    {
        public Task<bool> AddDevice(DevicesDTOs.AddDevice requestDto);
        public Task<bool> RemoveDevice(DevicesDTOs.RemoveDevice requestDto);
        public Task<List<RtspStreamToSimId>> StartDeviceChanneles(DevicesDTOs.StartDeviceChanneles requestDto);
        public Task<bool> StopDeviceChanneles(DevicesDTOs.StopDeviceChanneles requestDto);


        Task<List<RtspStreamToSimId>> StartAllDevicesChannelsAsync();
        Task<bool> StopAllDevicesChannelsAsync();
        Task<IEnumerable<string>> GetAllDevicesAsync();
    }
}
