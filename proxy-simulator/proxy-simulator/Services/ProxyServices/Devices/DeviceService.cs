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
        public async Task<bool> AddDevice(DevicesDTOs.AddDevice requestDto)
        {
            bool res = await this._multimediaServiceAPI.UploadFileAsync(requestDto.multimediaFile.OpenReadStream(),requestDto.multimediaFile.FileName);
            return res;
        }

        public async Task<bool> RemoveDevice(DevicesDTOs.RemoveDevice requestDto)
        {
            MultimediaApiDTO.DeleteFileDTO dto = new MultimediaApiDTO.DeleteFileDTO { FileName = "klv_metadata_test_sync.ts" };
            bool res = await this._multimediaServiceAPI.DeleteFileAsync(dto);
            return res;
        }

        public async Task<bool> StartDeviceChanneles(DevicesDTOs.StartDeviceChanneles requestDto)
        {
            MultimediaApiDTO.StartStreamDTO dto = new MultimediaApiDTO.StartStreamDTO { FileName = "klv_metadata_test_sync.ts", SourceFileId = 3};
            bool res = await this._multimediaServiceAPI.StartStreamAsync(dto);
            return res;
        }

        public async Task<bool> StopDeviceChanneles(DevicesDTOs.StopDeviceChanneles requestDto)
        {
            MultimediaApiDTO.StopStreamDTO dto = new MultimediaApiDTO.StopStreamDTO { StreamName = "klv_metadata_test_sync.ts" };
            bool res = await this._multimediaServiceAPI.StopStreamAsync(dto);
            return res;
        }
    }
}
