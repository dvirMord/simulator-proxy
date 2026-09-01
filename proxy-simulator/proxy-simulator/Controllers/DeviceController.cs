using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using proxy_simulator.Constants;
using proxy_simulator.DTOs;
using proxy_simulator.Interfaces;
using static proxy_simulator.DTOs.DevicesDTOs;

namespace proxy_simulator.Controllers
{
    [ApiController]
    [Route("api/v1.0/sp")]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;
        private readonly ILogger<DeviceController> _logger;

        public DeviceController(IDeviceService deviceService, ILogger<DeviceController> logger)
        {
            _deviceService = deviceService;
            _logger = logger;
        }

        //==============================Devices==============================================================
        [HttpPost("Devices")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> AddDevice([FromForm] DevicesDTOs.AddDevice requestDto)
        {
            _logger.LogInformation(ControllersLogs.Device.ADD_DEVICE_ACTIVATED,requestDto.TelemetryFile.FileName, requestDto.MultimediaFile.FileName);

            var result = await _deviceService.AddDevice(requestDto);
            return Ok(new { success = result });
        }

        [HttpDelete("Devices")]
        public async Task<IActionResult> RemoveDevice([FromBody] DevicesDTOs.RemoveDevice requestDto)
        {
            _logger.LogInformation(ControllersLogs.Device.REMOVE_DEVICE_ACTIVATED, requestDto.deviceName);

            var result = await _deviceService.RemoveDevice(requestDto);
            return Ok(new { success = result });
        }

        [HttpGet("Devices")]
        public async Task<IActionResult> GetAllDevices()
        {
            _logger.LogInformation(ControllersLogs.Device.GET_ALL_DEVICES_ACTIVATED);
            IEnumerable<string> devices = await this._deviceService.GetAllDevicesAsync();
            return Ok(new { msg = true, Devices = devices});
        }

        //==============================Channels=============================================================
        [HttpPost("Devices/Start")]
        public async Task<IActionResult> StartDeviceChanneles([FromBody] DevicesDTOs.StartDeviceChanneles requestDto)
        {
            _logger.LogInformation(ControllersLogs.Device.START_DEVICE_CHANNELS_ACTIVATED, requestDto.deviceName);

            var streams = await _deviceService.StartDeviceChanneles(requestDto);
            return Ok(new { success = true , Streams = streams });
        }

        [HttpPost("Devices/Stop")]
        public async Task<IActionResult> StopDeviceChanneles([FromBody] DevicesDTOs.StopDeviceChanneles requestDto)
        {
            _logger.LogInformation(ControllersLogs.Device.STOP_DEVICE_CHANNELS_ACTIVATED, requestDto.deviceName);

            var result = await _deviceService.StopDeviceChanneles(requestDto);
            return Ok(new { success = result });
        }

        [HttpPost("Devices/StartAll")]
        public async Task<IActionResult> StartAllDevicesChannels()
        {
            _logger.LogInformation(ControllersLogs.Device.START_ALL_DEVICES_CHANNELS_ACTIVATED);
            try
            {
                List<RtspStreamToSimId> result = await _deviceService.StartAllDevicesChannelsAsync();
                return Ok(new
                {
                    success = true,
                    message = ServicesLogs.Device.MSG_START_ALL_DEVICES_SUCCESS,
                    Streams = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost("Devices/StopAll")]
        public async Task<IActionResult> StopAllDevicesChannels()
        {
            _logger.LogInformation(ControllersLogs.Device.STOP_ALL_DEVICES_CHANNELS_ACTIVATED);
            try
            {
                bool result = await _deviceService.StopAllDevicesChannelsAsync();
                return Ok(new
                {
                    success = result,
                    message = ServicesLogs.Device.MSG_STOP_ALL_DEVICES_SUCCESS
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet("Devices/ActiveStreams")]
        public async Task<IActionResult> GetActiveStreamsAsync()
        {
            var activeStreams = await _deviceService.GetActiveStreamsAsync();
            return Ok(new { success = true, Streams = activeStreams });
        }
        //====================================END============================================================
    }
}