using proxy_simulator.DTOs;
using proxy_simulator.Interfaces;

namespace proxy_simulator.Services
{
    public class DeviceService : IDeviceService
    {
        //==================Simulators Services=========================
        private readonly IMultimediaServiceAPI _multimediaServiceAPI;
        private readonly ITelemetryServiceAPI _telemetryServiceAPI;
        private readonly ILogger<DeviceService> _logger;
        private readonly IDBService _dBService;
        
        //=========================END==================================

        public DeviceService(
            ILogger<DeviceService> logger,
            IMultimediaServiceAPI multimediaServiceAPI,
            ITelemetryServiceAPI telemetryServiceAPI)
        private readonly ILogger<DeviceService> _logger;

        public DeviceService(ILogger<DeviceService> looger, IMultimediaServiceAPI multimediaServiceAPI,
            IDBService dBService)
        {
            this._multimediaServiceAPI = multimediaServiceAPI;
            this._logger = looger;
            this._dBService = dBService;
            _multimediaServiceAPI = multimediaServiceAPI;
            _telemetryServiceAPI = telemetryServiceAPI;
            _logger = logger;
        }

        //=================Interface Implementations====================
        public async Task<bool> AddDevice(DevicesDTOs.AddDevice requestDto)
        {
            await using var multimediaStream = requestDto.MultimediaFile.OpenReadStream();
            await using var telemetryStream = requestDto.TelemetryFile.OpenReadStream();

            var uploadMultimediaTask = _multimediaServiceAPI.UploadFileAsync(
                multimediaStream,
                requestDto.MultimediaFile.FileName
            );

            var uploadTelemetryTask = _telemetryServiceAPI.UploadKlvFileAsync(
                telemetryStream,
                requestDto.TelemetryFile.FileName
            );

            var results = await Task.WhenAll(uploadMultimediaTask, uploadTelemetryTask);
            return results[0] && results[1];
        }

        public async Task<bool> RemoveDevice(DevicesDTOs.RemoveDevice requestDto)
        {
            var multimediaDto = new MultimediaApiDTO.DeleteFileDTO
            {
                FileName = requestDto.deviceName
            };

            var telemetryDto = new TelemetryApiDTO.DeleteFileDTO
            {
                FileName = requestDto.deviceName
            };

            var deleteMultimediaTask = _multimediaServiceAPI.DeleteFileAsync(multimediaDto);
            var deleteTelemetryTask = _telemetryServiceAPI.DeleteKlvFileAsync(telemetryDto);

            var results = await Task.WhenAll(deleteMultimediaTask, deleteTelemetryTask);
            return results[0] && results[1];
        }

        public async Task<bool> StartDeviceChanneles(DevicesDTOs.StartDeviceChanneles requestDto)
        {
            var multimediaDto = new MultimediaApiDTO.StartStreamDTO
            {
                FileName = requestDto.deviceName,
                SourceFileId = 1,
                Type = MultimediaApiDTO.StreamType.Video
            };

            var telemetryDto = new TelemetryApiDTO.StartStreamDTO
            {
                FileName = "truck_decoded.txt"
            };

            var startMultimediaTask = _multimediaServiceAPI.StartStreamAsync(multimediaDto);
            var startTelemetryTask = _telemetryServiceAPI.StartStreamAsync(telemetryDto);

            await Task.WhenAll(startMultimediaTask, startTelemetryTask);

            bool isMultimediaOk = await startMultimediaTask;
            var telemetryResponse = await startTelemetryTask;

            return isMultimediaOk && (telemetryResponse is not null && telemetryResponse.Success);
        }

        public async Task<IEnumerable<string>> GetAllDevicesAsync()
        {
            return await this._dBService.GetAllDevicesAsync();
        }

        public async Task<bool> StopDeviceChanneles(DevicesDTOs.StopDeviceChanneles requestDto)
        {
            var multimediaDto = new MultimediaApiDTO.StopStreamDTO
            {
                StreamName = requestDto.deviceName
            };

            var telemetryDto = new TelemetryApiDTO.StopStreamDTO
            {
                FileName = requestDto.deviceName
            };

            var stopMultimediaTask = _multimediaServiceAPI.StopStreamAsync(multimediaDto);
            var stopTelemetryTask = _telemetryServiceAPI.StopStreamAsync(telemetryDto);

            await Task.WhenAll(stopMultimediaTask, stopTelemetryTask);

            bool isMultimediaOk = await stopMultimediaTask;
            var telemetryResponse = await stopTelemetryTask;

            return isMultimediaOk && (telemetryResponse is not null && telemetryResponse.Success);
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