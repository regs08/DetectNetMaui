using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DetectApp
{
    public class PingPongTester
    {
        private MqttClient _mqttClient;
        private string _brokerAddress;
        private string _pingTopic = "ping";

        public PingPongTester(ServerConfig config)
        {
            _brokerAddress = config.IPAddress;
            _mqttClient = new MqttClient(_brokerAddress);
            _mqttClient.MqttMsgPublishReceived += OnMessageReceived;
        }

        public bool Connect()
        {
            if (!_mqttClient.IsConnected)
            {
                string clientId = Guid.NewGuid().ToString();
                _mqttClient.Connect(clientId);
            }
            return _mqttClient.IsConnected;
        }

        public void Subscribe()
        {
            if (_mqttClient.IsConnected)
            {
                _mqttClient.Subscribe(new string[] { _pingTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            }
        }

        public void SendPing()
        {
            if (_mqttClient.IsConnected)
            {
                Console.WriteLine("Sending ping...");
                _mqttClient.Publish(_pingTopic, System.Text.Encoding.UTF8.GetBytes("ping"));
            }
        }

        private void OnMessageReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string receivedMessage = System.Text.Encoding.UTF8.GetString(e.Message);
            if (e.Topic == _pingTopic && receivedMessage == "pong")
            {
                Console.WriteLine("Pong received!");
            }
        }

        public void Disconnect()
        {
            if (_mqttClient.IsConnected)
            {
                _mqttClient.Disconnect();
            }
        }
    }
}
