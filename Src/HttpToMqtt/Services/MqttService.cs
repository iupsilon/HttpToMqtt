using System;
using System.Threading;
using System.Threading.Tasks;
using HttpToMqtt.Model;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Publishing;

namespace HttpToMqtt.Services
{
    public class MqttService
    {
        private MqttConfigurationModel _mqttConfiguration;

        public MqttService(IConfiguration configuration)
        {
            _mqttConfiguration = configuration.GetSection("Mqtt").Get<MqttConfigurationModel>();
        }
        public async Task PublishAsync(string topic, string payload)
        {
            // Get mqtt configuration model

            // Create a new MQTT client.
            var factory = new MqttFactory();
            using (var mqttClient = factory.CreateMqttClient())
            {
                var mqttOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer(_mqttConfiguration.Server, _mqttConfiguration.Port) // Port is optional
                    .WithCredentials(_mqttConfiguration.Username, _mqttConfiguration.Password)
                    .Build();

                var mqttMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(payload)
                    .WithExactlyOnceQoS()
                    .WithRetainFlag()
                    .Build();

                var connectResult = await mqttClient.ConnectAsync(mqttOptions, new CancellationTokenSource(TimeSpan.FromSeconds(_mqttConfiguration.ConnectTimeoutSeconds)).Token);
                if (connectResult.ResultCode != MqttClientConnectResultCode.Success)
                {
                    throw new InvalidOperationException($"Mqtt connection error: {connectResult.ReasonString} ({connectResult.ResultCode})");
                }

                var publishResult = await mqttClient.PublishAsync(mqttMessage, new CancellationTokenSource(TimeSpan.FromSeconds(_mqttConfiguration.ConnectTimeoutSeconds)).Token);

                if (publishResult.ReasonCode != MqttClientPublishReasonCode.Success)
                {
                    throw new InvalidOperationException($"Mqtt PublishAsync error: {publishResult.ReasonString} ({publishResult.ReasonCode})");
                }
            }
        }
    }
}