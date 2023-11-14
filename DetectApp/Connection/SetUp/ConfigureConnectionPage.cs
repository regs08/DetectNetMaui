using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace DetectApp
{
    public class ConfigureConnectionPage : ContentPage
    {
        //private MQTTConnectionService _mqttConnectionService;
        private PingPongTester _pingService;
        private ServerConfig _config;
        private Button _connectToMQTTButton;
        private Button _textReceiveButton;
        private Button _imageReceiveButton;

        public ConfigureConnectionPage(ServerConfig config)
        {
            _config = config;
            _pingService = new PingPongTester(_config);
            InitializeUI();
        }
        private void InitializeUI()
        {
            var titleLabel = new Label
            {
                Text = $"Setup Connection for {_config.ServerName}",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };

            // Create the buttons
            var (connectToMQTTButton, textReceiveButton, imageReceiveButton) = UIComponents.CreateMQTTAndServerButtons(_config);

            // Attach event handlers
            connectToMQTTButton.Clicked += OnConnectToMQTTClicked;
            textReceiveButton.Clicked += OnTextReceiveClicked;
            imageReceiveButton.Clicked += OnImageReceiveClicked;

            // Assign buttons to class variables
            _connectToMQTTButton = connectToMQTTButton;
            _textReceiveButton = textReceiveButton;
            _imageReceiveButton = imageReceiveButton;

            var stackLayout = new StackLayout
            {
                Children =
        {
            titleLabel,
            _connectToMQTTButton,
            _textReceiveButton,
            _imageReceiveButton
            // Add other UI elements here...
        }
            };

            Content = stackLayout;
        }

        // Assuming you have buttons setup and wired to these methods
        [Obsolete]
        async void OnConnectToMQTTClicked(object sender, EventArgs e)
        {
            bool isConnected =  _pingService.Connect();
            if (isConnected)
            {
                Console.WriteLine("CONNECTED");
                // Subscribe to the response topic
                _pingService.Subscribe();

                // Enable other buttons if needed
                _textReceiveButton.IsEnabled = true;
                _imageReceiveButton.IsEnabled = true;

                // Change button colors to indicate they are enabled
                _textReceiveButton.BackgroundColor = Colors.Green;
                _imageReceiveButton.BackgroundColor = Colors.Green;
                MessagingCenter.Send(this, MessagingConstants.MQTTConnectionEstablished, _config.ServerName);

            }
            else
            {
                Console.WriteLine("Not Connected");
            }
        }


        async void OnTextReceiveClicked(object sender, EventArgs e)
        {
            _pingService.SendPing();
        }

        async void OnImageReceiveClicked(object sender, EventArgs e)
        {
            // Send a request for an image
           // _mqttConnectionService.MQTTClient.Publish("requestTopic", "getImage");

            // Listen for response and handle it
        }


    }

}
