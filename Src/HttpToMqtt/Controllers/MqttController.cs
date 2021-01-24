using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using HttpToMqtt.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
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
                Log.Information($"Publish deviceName: {deviceName} action: {action} payload: {payload}");
                var mqttTopic = $"{deviceName}/{action}";
                await _mqttService.PublishAsync(mqttTopic, payload);

                Log.Information($"Publish success (topic: {mqttTopic})");

                return Ok(mqttTopic);
            }
            catch (Exception e)
            {
                Log.Error($"Publish error", e);
                return StatusCode(500, "Publish error, see log");
            }
        }
    }
}