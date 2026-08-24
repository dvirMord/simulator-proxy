//---------Microsoft space-------------
using Microsoft.AspNetCore.Mvc;
//-------------END----------------------

//--------porxy simulatur space---------
using proxy_simulator.Interfaces;
using proxy_simulator.DTOs;
//-------------END----------------------


namespace proxy_simulator.Controllers
{
    [ApiController]
    [Route("api/v1.0/ts")]// add Versioning
    public class ProxyController: ControllerBase
    {
        //----------dependency injection-------------
        private readonly IProxyService _porxyService;
        //------------------END----------------------

        public ProxyController(IProxyService proxyService) 
        {
            this._porxyService = proxyService;
        }

        //--------------For the list of channels in the CLI----------------
        [HttpGet("Channels/ms")]
        public async Task<IActionResult> GetAllMultimediaChannels()
        {
            //channels = this._proxyService.GetAllMultimediaChannelsAPI();
            return Ok(new { MultimediaChanells = "Working on it...." });
        }

        [HttpGet("channels/ts")]
        public async Task<IActionResult> GetAllTelemetryChannels()
        {
            //channels = this._proxyService.GetAllTelemetryChannelsAPI();
            return Ok(new { TelemetryChanells = "Working on it...." });
        }
        //-----------------------------END--------------------------------------

        //-------------------For Device's API-----------------------------------
        [HttpPost("Devices")]
        public async Task<IActionResult> AddDevice([FromBody] DevicesDTOs.AddDevice requestDto)
        {
            //channels = await this._proxyService.AddDeviceAPI();
            return Ok(new { state = "Working on it...." });
        }

        [HttpDelete("Devices")]
        public async Task<IActionResult> RemoveDevice([FromBody] DevicesDTOs.RemoveDevice requestDto)
        {
            //channels = await this._proxyService.AddDeviceAPI();
            return Ok(new { state = "Working on it...." });
        }

        [HttpPatch("Devices")]
        public async Task<IActionResult> UpdateDeviceName([FromBody] DevicesDTOs.UpdateDevice requestDto)
        {
            //channels = await this._proxyService.AddDeviceAPI();
            return Ok(new { state = "Working on it...." });
        }
        //-----------------------------END--------------------------------------

        //-------------------For Channle's API----------------------------------
        
        //-----------------------------END--------------------------------------
    }
}
