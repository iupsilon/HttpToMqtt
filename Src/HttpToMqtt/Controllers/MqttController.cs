using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HttpToMqtt.Services;

using Microsoft.AspNetCore.Mvc;

using Serilog;

namespace HttpToMqtt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MqttController : ControllerBase
    {
        private readonly MqttService _mqttService;


        public MqttController(MqttService mqttService)
        {
            _mqttService = mqttService;
        }

        [HttpGet]
        [Route("publish")]
        public async Task<IActionResult> Publish([Required] string deviceName, [Required] string action, string payload)
        {
            try
            {
                Log.Information("Publish deviceName: {DeviceName} action: {Action} payload: {Payload}", deviceName, action, payload);
                var mqttTopic = $"{deviceName}/{action}";
                await _mqttService.PublishAsync(mqttTopic, payload);

                Log.Information("Publish success (topic: {MqttTopic})", mqttTopic);

                return Ok(mqttTopic);
            }
            catch (Exception e)
            {
                Log.Error(e, "Publish error: {Error}", e);
                return StatusCode(500, "Publish error, see log");
            }
        }
    }
}