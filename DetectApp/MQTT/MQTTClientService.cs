using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Threading;

namespace DetectApp
{
    public class MQTTConnectionService
    {
        public MQTTClient MQTTClient { get; private set; }
        private readonly object _pingLock = new object();
        private bool _isPingResponseReceived;

        public MQTTConnectionService(ServerConfig config)
        {
            MQTTClient = new MQTTClient(config.IPAddress);
        }

        public async Task<bool> ConnectToMQTTAsync()
        {
            try
            {
                string clientId = Guid.NewGuid().ToString();
                MQTTClient.Connect();
                Console.WriteLine("Connected to MQTT Broker.");
                return MQTTClient.IsConnected;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to MQTT Broker: {ex.Message}");
                return false;
            }
        }

        public void SubscribeToTopics(IEnumerable<string> topics)
        {
            foreach (var topic in topics)
            {
                MQTTClient.SubscribeToTopic(topic);
                Console.WriteLine($"Subscribed to topic: {topic}");
            }
        }

        private string HandleReceivedMessage(object sender, MqttMsgPublishEventArgs e)
        {
            var topic = e.Topic;
            var message = e.Message;

            if (topic == "pingpong")
            {
                Console.WriteLine("Received 'pingpong' message.");
                return message.ToString();
            }

            Console.WriteLine("Topic not subscribed.");
            return "Topic not subscribed";
        }

        public async Task<bool> SendPingRequestAsync(TimeSpan timeout)
        {
            if (!MQTTClient.IsConnected)
            {
                Console.WriteLine("MQTT client is not connected. Cannot send ping request.");
                return false;
            }

            lock (_pingLock)
            {
                _isPingResponseReceived = false;
            }

            //MQTTClient.Ping();
            Console.WriteLine("Ping Request sent.");

            // Here we just wait for the specified timeout as there's no direct event for ping response.
            // If the client remains connected, we assume the ping response was received.
            await Task.Delay(timeout);

            lock (_pingLock)
            {
                _isPingResponseReceived = MQTTClient.IsConnected;
            }

            return _isPingResponseReceived;
        }

        public void DisconnectFromMQTT()
        {
            MQTTClient.Disconnect();
            Console.WriteLine("Disconnected from MQTT Broker.");
        }
    }
}
