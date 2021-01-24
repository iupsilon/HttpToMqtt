using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;

namespace HttpToMqtt.Services
{
    public class MqttService
    {
        private readonly IConfiguration _configuration;

        public MqttService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task PublishAsync(string topic, string payload)
        {


            _configuration.GetSection("");
            // Create a new MQTT client.
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();

            // Use TCP connection.
            var mqttOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("broker.hivemq.com", 1883) // Port is optional
                .WithCredentials("bud", "%spencer%")
                .Build();

            var mqttMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            var connectResult = await mqttClient.ConnectAsync(mqttOptions, new CancellationTokenSource(TimeSpan.FromSeconds(60)).Token);
            if (connectResult.ResultCode != MqttClientConnectResultCode.Success)
            {
                throw new InvalidOperationException($"Mqtt connection error: {connectResult.ReasonString} ({connectResult.ResultCode})")M
            }


            var publishResult = await mqttClient.PublishAsync(mqttMessage, new CancellationTokenSource(TimeSpan.FromSeconds(60)).Token);
        }
    }
}