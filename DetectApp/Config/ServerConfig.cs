using System;
using Microsoft.Maui.Controls;

namespace DetectApp
{
    public class ServerConfig
    {
        public string ServerName { get; set; }
        public string IPAddress { get; set; }
        public string VideoIPAddress { get; set; }
        public string PortNumber { get; set; }
        public List <string> SelectedLabels { get; set;}
        public List <string> AvailableLabels { get; set; }
        public List <string> SubscriptionTopics { get; set; }
        public List<string> AvailibleTopics { get; set; }

        public double ConfidenceThreshold { get; set;}
        public string VideoStreamUrl
        {
            get
            {
                return $"http://{VideoIPAddress}:{PortNumber}/video";
            }
        }
        public ServerConfig()
        {
            ServerName = "DefaultServer(Local Host)";
            IPAddress = "10.0.2.2";
            VideoIPAddress = "10.0.2.2";
            PortNumber = "8080";
            SelectedLabels = new List<string> { "person" };
            AvailableLabels = new List<string> { "person", "dog", "couch", "cellphone" };
            SubscriptionTopics = new List<string> { "vehicle/detections", "vehicle/image" };

            ConfidenceThreshold = 0.51f;
        }
    }
}

