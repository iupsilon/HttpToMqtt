using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;

namespace HttpToMqtt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MqttController : ControllerBase
    {
        public async Task<IActionResult> Publish([Required] string deviceName, [Required] string action)
        {
            var mqttTopic = $"{deviceName}/{action}";


         
            
            

            return Ok(mqttTopic);
        }
    }
}