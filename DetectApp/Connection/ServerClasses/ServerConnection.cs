using System;
 
namespace DetectApp
{
    public class ServerConnection
    {
        public string _ipaddress;
        public MQTTClient _mqttClient;
        public ServerConfig _config;

        public MQTTClient MQTTClient => _mqttClient;
        public ServerConfig Config => _config;
        public string IPAddress => _ipaddress;

        public ServerConnection(ServerConfig config)
        {
            _config = config;
            _ipaddress = config.IPAddress;
            _mqttClient = new MQTTClient(_ipaddress);
            _mqttClient.Connect();
            
        }

        public static bool ConnectToVideo()
        {
            return true; // Placeholder
        }

        public bool ConnectToMQTT()
        {
            try
            {
                _mqttClient.Connect();
                Console.WriteLine($"{_mqttClient.IsConnected}, {_mqttClient.SubscribeToTopic(_ipaddress)}");
                return _mqttClient.IsConnected && _mqttClient.SubscribeToTopic(_ipaddress);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to MQTT: {ex.Message}");
                return false;
            }
        }

        public static bool TryConnectToServer()
        {
            return true; // Placeholder
        }

        public static void DisconnectFromVideo()
        {
            // Placeholder logic for disconnecting from the video server
            Console.WriteLine("Disconnected from Video Server.");
        }

        public static void DisconnectFromMQTT()
        {
            // Placeholder logic for disconnecting from the MQTT server
            Console.WriteLine("Disconnected from MQTT Server.");
        }

    }

}



