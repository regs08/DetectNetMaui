using System;
using System.Collections.Generic;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Text;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
/*
 * todo two way communication to upload the photos being sent. e.g when the config 
 * is updated to only 'couch' only couch phots are sent 
 * todo get config from python side or from flask 
 */
namespace DetectApp
{
    public class MQTTClient
    {
        public ObservableCollection<ImageSource> RecentImages { get; private set; } = new ObservableCollection<ImageSource>();


        public bool IsConnected => _mqttClient?.IsConnected ?? false;

        private MqttClient _mqttClient;

        public event Action<List<LogEntry>> LogsReceived;
        public event Action<byte[]> ImageReceived;
        public event Action<string> ErrorOccurred;

        public MQTTClient(string brokerAddress)
        {
            try
            {
                _mqttClient = new MqttClient(brokerAddress);
                _mqttClient.MqttMsgPublishReceived += HandleReceivedMessage;
            }
            catch (SocketException ex)
            {
                ErrorOccurred?.Invoke($"Error initializing MQTT client: {ex.Message}");
            }
        }

        public void Connect()
        {
            if (_mqttClient != null)
            {
                string clientId = Guid.NewGuid().ToString();
                _mqttClient.Connect(clientId);
            }
            else
            {
                ErrorOccurred?.Invoke("MQTT client not initialized.");
            }
        }

        public bool SubscribeToTopic(string topic)
        {
            if (_mqttClient != null && _mqttClient.IsConnected)
            {
                _mqttClient.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
                return true;
            }
            else
            {
                Console.WriteLine("not connected to server");
                ErrorOccurred?.Invoke("MQTT client not connected.");
                return false;
            }
        }

        public void Disconnect()
        {
            _mqttClient?.Disconnect();
        }

        [Obsolete]

        // Todo update this method so its more dynamic e.g if it starts with image use from stream
        // todo storer these in a list frrom the config 
        private void HandleReceivedMessage(object sender, MqttMsgPublishEventArgs e)
        {
            // Check if the message is JSON or image data based on topic or content
            var topic = e.Topic;
            var message = e.Message;

            if (topic == "vehicle/detections")
            {
                var jsonMessage = Encoding.UTF8.GetString(message);
                var logMessages = JsonConvert.DeserializeObject<List<LogEntry>>(jsonMessage);

                LogsReceived?.Invoke(logMessages); // Invoke with the JSON message as a string
            }
            else if (topic == "vehicle/image")
            {
                _ = Device.InvokeOnMainThreadAsync(() =>
                {
                    var imageSource = ImageSource.FromStream(() => new MemoryStream(message));
                    UpdateRecentImages(imageSource);
                    ImageReceived?.Invoke(message);
                });
            }
           
            else
            {
                Console.WriteLine("not subscribed to topic");
                // Handle other topics or unknown content types as needed
                ErrorOccurred?.Invoke($"Received message on unknown topic: {topic}");
            }
        }

        private void UpdateRecentImages(ImageSource newImageSource)
        {
            if (RecentImages.Count == 2)
            {
                RecentImages.RemoveAt(0);
            }
            RecentImages.Add(newImageSource);
        }

            public void SubscribeToLogsReceived(Action<List<LogEntry>> handler) // Change the parameter type
        {
            LogsReceived += handler;
        }

        // Allow external classes to unsubscribe from the LogReceived event
        public void UnsubscribeFromLogReceived(Action<List<LogEntry>> handler) // Change the parameter type
        {
            LogsReceived -= handler;
        }
    }
}
