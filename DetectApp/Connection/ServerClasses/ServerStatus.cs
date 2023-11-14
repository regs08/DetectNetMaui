namespace DetectApp
{
    public class ServerStatus
    {
        public ServerConfig Config { get; set; }
        public bool IsConnectedMQTT { get; set; }
        public bool IsConnectedText { get; set; }
        public bool IsConnectedImage { get; set; }

        public ServerStatus(ServerConfig config)
        {
            Config = config;
            IsConnectedMQTT = false;
            IsConnectedText = false;
            IsConnectedImage = false;
        }
    }
}

