using proxy_simulator.DTOs;

namespace proxy_simulator.Interfaces
{
    public interface IDeviceService
    {
        public Task<bool> AddDevice(DevicesDTOs.AddDevice requestDto);
        public Task<bool> RemoveDevice(DevicesDTOs.RemoveDevice requestDto);
        public Task<bool> StartDeviceChanneles(DevicesDTOs.StartDeviceChanneles requestDto);
        public Task<bool> StopDeviceChanneles(DevicesDTOs.StopDeviceChanneles requestDto);
        
    }
}
