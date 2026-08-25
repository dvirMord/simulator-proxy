//---------Microsoft space-------------
using Microsoft.AspNetCore.Mvc;
//-------------END----------------------

//--------porxy simulatur space---------
using proxy_simulator.DTOs;
using proxy_simulator.Interfaces;
//-------------END----------------------


namespace proxy_simulator.Controllers
{
    [ApiController]
    [Route("api/v1.0/sp")]// add Versioning
    public class DeviceController : ControllerBase
    {
        //----------dependency injection-------------
        private readonly IDeviceService _deviceService;
        private readonly ILogger _logger;
        //------------------END----------------------

        public DeviceController(IDeviceService deviceService, ILogger<DeviceController> logger) 
        {
           this._deviceService = deviceService; 
           this._logger = logger;
        }

        //==============================Devices==============================================================
        [HttpPost("Devices")]
        public async Task<IActionResult> AddDevice(DevicesDTOs.AddDevice requestDto)
        { 
            this._logger.LogInformation($"[Controller->Device] AddDevice endpoint activated for {requestDto.telemetryFile.FileName} and {requestDto.multimediaFile.FileName}.");
            return Ok(new { succes = await this._deviceService.AddDevice(requestDto) });
        }

        [HttpDelete("Devices")]
        public async Task<IActionResult> RemoveDevice(DevicesDTOs.RemoveDevice requestDto)
        {
            this._logger.LogInformation($"[Controller->Device] RemoveDevice endpoint activated for {requestDto.deviceName}.");
            return Ok(new { succes = await this._deviceService.RemoveDevice(requestDto)});
        }

        [HttpGet("Devices")]
        public async Task<IActionResult> GetAllDevices()
        {
            this._logger.LogInformation("[Controller->Device] GetAllDevices endpoint activated.");
            return Ok(new { msg = "GetAllDevices RemoveDevice Working on it...." });
        }
        
        //==============================Channels=================================================================
        [HttpPost("Devices/Start")]
        public async Task<IActionResult> StartDeviceChanneles(DevicesDTOs.StartDeviceChanneles requestDto)
        {
            this._logger.LogInformation($"[Controller->Device] StartDeviceChanneles endpoint activated for {requestDto.deviceName}.");
            return Ok(new { succes = await this._deviceService.StartDeviceChanneles(requestDto) });
        }

        [HttpPost("Devices/Stop")]
        public async Task<IActionResult> StopDeviceChanneles(DevicesDTOs.StopDeviceChanneles requestDto)
        {
            this._logger.LogInformation($"[Controller->Device] StopDeviceChanneles endpoint activated for {requestDto.deviceName}.");
            return Ok(new { succes = await this._deviceService.StopDeviceChanneles(requestDto) });
        }

        [HttpPost("Devices/StartAll")]
        public async Task<IActionResult> StartAllDevicesChanneles()
        {
            this._logger.LogInformation("[Controller->Device] StartAllDevicesChanneles endpoint activated.");
            return Ok(new { msg = "StartAllDevicesChanneles RemoveDevice Working on it...." });
        }

        [HttpPost("Devices/StopAll")]
        public async Task<IActionResult> StopAllDevicesChanneles()
        {
            this._logger.LogInformation("[Controller->Device] StopAllDevicesChanneles endpoint activated.");
            return Ok(new { msg = "StopAllDevicesChanneles RemoveDevice Working on it...." });
        }
        //====================================END==================================================================
    }
}
