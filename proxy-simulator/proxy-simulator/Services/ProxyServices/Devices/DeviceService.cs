using proxy_simulator.Constants;
using proxy_simulator.DTOs;
using proxy_simulator.Interfaces;
using static proxy_simulator.Constants.ServicesConstants.SQlite;
using static proxy_simulator.Constants.ServicesConstants.SQlite.ChannelType;
using static proxy_simulator.DTOs.DevicesDTOs;

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

            int multimediaSourceFileId = await _multimediaServiceAPI.UploadFileAsync(
                multimediaStream,
                requestDto.MultimediaFile.FileName
            );

            await this._dBService.InsertChannelAsync(ChannelType.Multimedia, multimediaSourceFileId, requestDto.DeviceName);

            int telemetrySourceFileId = await _telemetryServiceAPI.UploadKlvFileAsync(
                telemetryStream,
                requestDto.TelemetryFile.FileName
            );
            await this._dBService.InsertChannelAsync(ChannelType.Telemetry, telemetrySourceFileId, requestDto.DeviceName);

            return true;
        }

        public async Task<bool> RemoveDevice(DevicesDTOs.RemoveDevice requestDto)
        {
            IEnumerable<ChannelSimInfo> deviceChannels = await this._dBService.GetChannelSimsByDeviceNameAsync(requestDto.deviceName);

            if (deviceChannels == null || !deviceChannels.Any())
            {
                return false;
            }

            foreach (ChannelSimInfo channel in deviceChannels)
            {
                switch (channel.Type)
                {
                    case ChannelType.Telemetry:
                        bool telemetryDeleted = await this._telemetryServiceAPI.DeleteKlvFileAsync(
                            new DTOs.TelemetryApiDTO.DeleteFileDTO { SimId = channel.SimId }
                        );
                        if (!telemetryDeleted)
                        {
                            throw new InvalidOperationException(
                                string.Format(ServicesLogs.Device.EXC_REMOVE_DEVICE_FAILED, requestDto.deviceName)
                            );
                        }
                        break;

                    case ChannelType.Multimedia:
                        bool multimediaDeleted = await this._multimediaServiceAPI.DeleteFileAsync(
                            new DTOs.MultimediaApiDTO.DeleteFileDTO { SimId = channel.SimId }
                        );
                        if (!multimediaDeleted)
                        {
                            throw new InvalidOperationException(
                                string.Format(ServicesLogs.Device.EXC_REMOVE_DEVICE_FAILED, requestDto.deviceName)
                            );
                        }
                        break;

                    default:
                        this._logger.LogWarning(ServicesLogs.Device.UNKNOWN_CHANNEL_TYPE,
                            channel.Type, channel.SimId, requestDto.deviceName);
                        break;
                }
            }
            bool isDeletedFromDb = await this._dBService.DeleteDeviceAsync(requestDto.deviceName);
            if (!isDeletedFromDb)
            {
                throw new InvalidOperationException(
                    string.Format(ServicesLogs.Device.EXC_REMOVE_DEVICE_FAILED, requestDto.deviceName)
                );
            }

            return true;
        }

        public async Task<bool> StartDeviceChanneles(DevicesDTOs.StartDeviceChanneles requestDto)
        {
            IEnumerable<ChannelSimInfo> deviceChannels = await this._dBService.GetChannelSimsByDeviceNameAsync(requestDto.deviceName);

            if (deviceChannels == null || !deviceChannels.Any())
            {
                return false;
            }

            foreach (ChannelSimInfo channel in deviceChannels)
            {
                switch (channel.Type)
                {
                    case ChannelType.Telemetry:
                        await this._telemetryServiceAPI.StartStreamAsync(
                            new DTOs.TelemetryApiDTO.StartStreamDTO { SimId = channel.SimId }
                        );
                        break;

                    case ChannelType.Multimedia:
                        await this._multimediaServiceAPI.StartStreamAsync(
                            new DTOs.MultimediaApiDTO.StartStreamDTO { SimId = channel.SimId }
                        );
                        break;

                    default:
                        this._logger.LogWarning(ServicesLogs.Device.UNKNOWN_CHANNEL_TYPE,
                            channel.Type, channel.SimId, requestDto.deviceName);
                        break;
                }
            }

            return true;
        }

        public async Task<IEnumerable<string>> GetAllDevicesAsync()
        {
            return await this._dBService.GetAllDevicesAsync();
        }

        public async Task<bool> StopDeviceChanneles(DevicesDTOs.StopDeviceChanneles requestDto)
        {
            IEnumerable<ChannelSimInfo> deviceChannels = await this._dBService.GetChannelSimsByDeviceNameAsync(requestDto.deviceName);

            if (deviceChannels == null || !deviceChannels.Any())
            {
                return false;
            }

            foreach (ChannelSimInfo channel in deviceChannels)
            {
                switch (channel.Type)
                {
                    case ChannelType.Telemetry:
                        await this._telemetryServiceAPI.StopStreamAsync(
                            new DTOs.TelemetryApiDTO.StopStreamDTO { SimId = channel.SimId }
                        );
                        break;

                    case ChannelType.Multimedia:
                        await this._multimediaServiceAPI.StopStreamAsync(
                            new DTOs.MultimediaApiDTO.StopStreamDTO { SimId = channel.SimId }
                        );
                        break;

                    default:
                        this._logger.LogWarning(ServicesLogs.Device.UNKNOWN_CHANNEL_TYPE,
                            channel.Type, channel.SimId, requestDto.deviceName);
                        break;
                }
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