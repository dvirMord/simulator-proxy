using proxy_simulator.DTOs;
using proxy_simulator.Interfaces;

namespace proxy_simulator.Services
{
    public class DeviceService: IDeviceService 
    {
        //==================Simulators Services=========================
        private readonly IMultimediaServiceAPI _multimediaServiceAPI;
        
        //=========================END==================================

        private readonly ILogger<DeviceService> _logger;

        public DeviceService(ILogger<DeviceService> looger, IMultimediaServiceAPI multimediaServiceAPI)
        {
            this._multimediaServiceAPI = multimediaServiceAPI;
            this._logger = looger;
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

        public async Task<bool> StopDeviceChanneles(DevicesDTOs.StopDeviceChanneles requestDto)
        { 
            return true;
        }
    }
}
