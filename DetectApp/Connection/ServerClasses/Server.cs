using System.Collections.ObjectModel;

namespace DetectApp
{
    public class Server
    {
        public ServerConnection Connection { get; }
        public ServerConfig Config { get; }

        public string IPAddress => Connection.IPAddress;
        public MQTTClient MQTTClient => Connection.MQTTClient;
        public ObservableCollection<ImageSource> RecentImages => Connection.MQTTClient.RecentImages;

        public Server(ServerConnection connection)
        {
            Connection = connection;
            Config = Connection.Config;

            MQTTClient.ImageReceived += OnImageReceived;
            SubscribeToMqttTopics();

        }

        public void UpdateLogConfig(List<string> selectedLabels, double confidenceThreshold)
        {

            Config.SelectedLabels = selectedLabels;
            Config.ConfidenceThreshold = confidenceThreshold;
        }

        private void OnImageReceived(byte[] imageData)
        {
            // Update your list of recent images
            // Assuming RecentImages is a List<ImageSource> holding the image data
            if (RecentImages.Count >= 2)
                RecentImages.RemoveAt(0);  // Remove the oldest image if there are already 2 images

            // Convert byte array to ImageSource
            // Assume imageSource is your ImageSource object
            ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(imageData));

            // Create an Image object and set its Source property
            Image image = new Image
            {
                Source = imageSource
            };

            // Add the new image to the list of recent images
            RecentImages.Add(imageSource);
        }
        private void SubscribeToMqttTopics()
        {
            foreach (string topic in Config.SubscriptionTopics)
            {
                MQTTClient.SubscribeToTopic(topic);
            }
        }
    }
}
