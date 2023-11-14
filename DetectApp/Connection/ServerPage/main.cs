using System;
using System.Collections.ObjectModel;
using DetectApp.Views.Server;
// todo as of now when we navigate back to the main page we lose our data. Seems
// like were creating a new server page eventhough thebutton is still there

namespace DetectApp
{
    public class ServerPage : ContentPage

    {
        private readonly Server _server;

        private readonly VideoStreamClient _videoStreamClient;
        private readonly MQTTClient _mqttClient;
        private readonly ObservableCollection<string> _receivedLogs;

        public ServerPage(Server server)
        {
            _server = server;
            _videoStreamClient = new VideoStreamClient(server);
            _videoStreamClient.ConnectAsync();

            _mqttClient = server.MQTTClient;
            _receivedLogs = new ObservableCollection<string>();
            Title = $"Server: {server.IPAddress}";

            var serverLayout = CreateButtons(server);
            Content = serverLayout;

            SubscribeToEvents();
        }

        private StackLayout CreateButtons(Server server)
        {
            var serverLayout = new StackLayout();

            var updateViewLogConfigButton = new Button { Text = "Update/View Log Config" };
            updateViewLogConfigButton.Clicked += UpdateViewLogConfigButton_Clicked;
            serverLayout.Children.Add(updateViewLogConfigButton);

            var viewPhotosButton = new Button { Text = "View Photos" };
            viewPhotosButton.Clicked += (s, e) => ViewPhotos();
            serverLayout.Children.Add(viewPhotosButton);

            var viewLogButton = new Button { Text = "View Log" };
            viewLogButton.Clicked += (s, e) => ViewLogs();
            serverLayout.Children.Add(viewLogButton);

            var viewStreamButton = new Button { Text = "View Stream" };

            Console.WriteLine($"video stream IP: {server.Config.VideoStreamUrl}");
            viewStreamButton.Clicked += (s, e) => ViewVideoStream(server.Config.VideoStreamUrl);
            serverLayout.Children.Add(viewStreamButton);


            return serverLayout;
        }

        private void SubscribeToEvents()
        {
            _mqttClient.LogsReceived += OnLogReceived;
        }

        private async void OnLogReceived(List<LogEntry> logs)
        {
            LogParser logParser = new(logs);
           // Thinking here to pass in the config from the server. instead of updating the the server selected the labels we will update the config 
            List<string> parsedLogs = logParser.ParseLogs(filterLabels:_server.Config.SelectedLabels,
                confidenceThreshold:_server.Config.ConfidenceThreshold);

            foreach (string parsedLog in parsedLogs)
            {
                _receivedLogs.Add(parsedLog);
            }
        }

        private async void ViewLogs()
        {
            var logPage = new LogsPage(_receivedLogs);
            await Navigation.PushAsync(logPage);
        }

        private async void ViewPhotos()
        {
            var photoViewerPage = new PhotoViewerPage(_server.RecentImages);  
            await Navigation.PushAsync(photoViewerPage);
        }

        private async void UpdateViewLogConfigButton_Clicked(object sender, EventArgs e)
        {
            await DisplayLogConfig();
        }

        private async Task DisplayLogConfig()
        {
            var selectedLabels = string.Join(", ", _server.Config.SelectedLabels);
            var confidenceThreshold = _server.Config.ConfidenceThreshold.ToString("P0");

            await DisplayAlert("Log Config", $"Labels: {selectedLabels}\nConfidence Threshold: {confidenceThreshold}", "OK");

            var updateAction = await DisplayActionSheet("Update Log Config?", "Cancel", null, "Yes", "No");

            if (updateAction == "Yes")
            {
                await UpdateLogConfig();
            }
        }

        private async Task UpdateLogConfig()
        {
            var selectedLabels = await UserDialogHelper.GetSelectionFromUser(
                prompt: "Select Labels, ",
                items: _server.Config.AvailableLabels,
                selectionMode: SelectionMode.Multiple
            );

            double default_conf = _server.Config.ConfidenceThreshold;
            string conf_prompt = $"Select Confidence Threshold (default is: {default_conf}";

            var confidenceThreshold = await UserDialogHelper.GetDoubleFromUser(this,
                prompt: conf_prompt,
                defaultValue: default_conf);

            _server.Config.SelectedLabels = selectedLabels;
            _server.Config.ConfidenceThreshold = confidenceThreshold;

            // Display a confirmation alert
            await DisplayAlert("Log Config Updated", $"Labels: {string.Join(", ", selectedLabels)}\nConfidence Threshold: {confidenceThreshold:P0}", "OK");
        }
        private async void ViewVideoStream(string videoStreamUrl)
        {
            var videoStreamPage = new VideoStreamPage(videoStreamUrl);
            await Navigation.PushAsync(videoStreamPage);
        }

    }
}

