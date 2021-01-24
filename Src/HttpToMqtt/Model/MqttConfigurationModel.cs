namespace HttpToMqtt.Model
{
    public class MqttConfigurationModel
    {
        public string Server { get; set; }
        public int  Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int ConnectTimeoutSeconds { get; set; }
    }
}