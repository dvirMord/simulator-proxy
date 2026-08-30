using proxy_simulator.DTOs;
using proxy_simulator.Interfaces;

namespace proxy_simulator.Services
{
    public class DeviceService: IDeviceService 
    {
        //==================Simulators Services=========================
        private readonly IMultimediaServiceAPI _multimediaServiceAPI;
        private readonly IDBService _dBService;
        
        //=========================END==================================

        private readonly ILogger<DeviceService> _logger;

        public DeviceService(ILogger<DeviceService> looger, IMultimediaServiceAPI multimediaServiceAPI,
            IDBService dBService)
        {
            this._multimediaServiceAPI = multimediaServiceAPI;
            this._logger = looger;
            this._dBService = dBService;
        }
        
        //=================inhrted functions=========================================================================
        //Call me when you see it!
        public async Task<bool> AddDevice(DevicesDTOs.AddDevice requestDto)
        {
            return true;
        }

        public async Task<bool> RemoveDevice(DevicesDTOs.RemoveDevice requestDto)
        {
            return true;
        }

        public async Task<bool> StartDeviceChanneles(DevicesDTOs.StartDeviceChanneles requestDto)
        {
            return true;
        }

        public async Task<IEnumerable<string>> GetAllDevicesAsync()
        {
            return await this._dBService.GetAllDevicesAsync();
        }

        public async Task<bool> StopDeviceChanneles(DevicesDTOs.StopDeviceChanneles requestDto)
        { 
            return true;
        }

        public async Task<bool> StartAllDevicesChannelsAsync()
        {
            IEnumerable<string> devices = await this._dBService.GetAllDevicesAsync();
            if (devices == null || !devices.Any())
            {
                return false;
            }

            foreach (string deviceName in devices)
            {
                await StartDeviceChanneles(new DevicesDTOs.StartDeviceChanneles { deviceName = deviceName });
            }

            return true;
        }

        public async Task<bool> StopAllDevicesChannelsAsync()
        {
            IEnumerable<string> devices = await this._dBService.GetAllDevicesAsync();
            if (devices == null || !devices.Any())
            {
                return false;
            }

            foreach (string deviceName in devices)
            {
                await StopDeviceChanneles(new DevicesDTOs.StopDeviceChanneles { deviceName = deviceName });
            }

            return true;
        }
    }
}
