using proxy_simulator.Constants;
using proxy_simulator.DTOs;
using proxy_simulator.Interfaces;
using static proxy_simulator.Constants.ServicesConstants.SQlite;
using static proxy_simulator.Constants.ServicesConstants.SQlite.ChannelType;

namespace proxy_simulator.Services
{
    public class DeviceService : IDeviceService
    {
        //==================Simulators Services=========================
        private readonly IMultimediaServiceAPI _multimediaServiceAPI;
        private readonly ITelemetryServiceAPI _telemetryServiceAPI;
        private readonly IDBService _dBService;
        private readonly ILogger<DeviceService> _logger;
        
        //=========================END==================================

        public DeviceService(
            ILogger<DeviceService> logger,
            IMultimediaServiceAPI multimediaServiceAPI,
            ITelemetryServiceAPI telemetryServiceAPI,
            IDBService dBService)
        {
            this._multimediaServiceAPI = multimediaServiceAPI;
            this._telemetryServiceAPI = telemetryServiceAPI;
            this._logger = logger;
            this._dBService = dBService;
        }

        //=================Interface Implementations====================
        public async Task<bool> AddDevice(DevicesDTOs.AddDevice requestDto)
        {
            await using var multimediaStream = requestDto.MultimediaFile.OpenReadStream();
            await using var telemetryStream = requestDto.TelemetryFile.OpenReadStream();

            await this._dBService.InsertDeviceAsync(requestDto.DeviceName);

            int MultimediaSoureFileId = await _multimediaServiceAPI.UploadFileAsync(
                multimediaStream,
                requestDto.MultimediaFile.FileName
            );

            await this._dBService.InsertChannelAsync(ChannelType.Multimedia, MultimediaSoureFileId, requestDto.DeviceName);

            int TelemetrySoureFileId = await _telemetryServiceAPI.UploadKlvFileAsync(
                telemetryStream,
                requestDto.TelemetryFile.FileName
            );
            await this._dBService.InsertChannelAsync(ChannelType.Telemetry, TelemetrySoureFileId, requestDto.DeviceName);
            return true;
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

            if (!results[0] || !results[1])
            {
                throw new InvalidOperationException(
                    string.Format(ServicesLogs.Device.EXC_REMOVE_DEVICE_FAILED, requestDto.deviceName)
                );
            }

            return true;
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
                FileName = ServicesLogs.Device.DEFAULT_TELEMETRY_STREAM_FILE
            };

            var startMultimediaTask = _multimediaServiceAPI.StartStreamAsync(multimediaDto);
            var startTelemetryTask = _telemetryServiceAPI.StartStreamAsync(telemetryDto);

            await Task.WhenAll(startMultimediaTask, startTelemetryTask);

            bool isMultimediaOk = await startMultimediaTask;
            var telemetryResponse = await startTelemetryTask;

            if (!isMultimediaOk || telemetryResponse is null || !telemetryResponse.Success)
            {
                throw new InvalidOperationException(
                    string.Format(ServicesLogs.Device.EXC_START_CHANNELS_FAILED, requestDto.deviceName)
                );
            }

            return true;
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

            if (!isMultimediaOk || telemetryResponse is null || !telemetryResponse.Success)
            {
                throw new InvalidOperationException(
                    string.Format(ServicesLogs.Device.EXC_STOP_CHANNELS_FAILED, requestDto.deviceName)
                );
            }

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