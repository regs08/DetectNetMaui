
using Microsoft.Maui.Controls;

namespace DetectApp
{
    public partial class MainPage : ContentPage
    {
        private ConfigService _configService = new ConfigService();
        private StackLayout _configsLayout = new StackLayout();
        private List<string> _availableConfigs = new();

        // stores config and connection info. used to enable connect to server button and pass the config 
        private Dictionary<string, ServerStatus> _serverStatuses = new Dictionary<string, ServerStatus>(); // Add this line

        public MainPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<ConfigureConnectionPage, string>(this, MessagingConstants.MQTTConnectionEstablished, (sender, serverName) =>
            {
                if (_serverStatuses.TryGetValue(serverName, out ServerStatus status))
                {
                    Console.WriteLine($"{status}");
                    status.IsConnectedMQTT = true;
                    Console.WriteLine($"Message rerceived objects updated{status}");
                }

            });

            var loadConfigButton = UIComponents.CreateLoadConfigButton();
            loadConfigButton.Clicked += LoadConfigButton_Clicked;
            ButtonStackLayout.Children.Add(loadConfigButton);

            // This layout will hold the loaded configs
            ButtonStackLayout.Children.Add(_configsLayout);


            // Initialize configs after the page is loaded
            Appearing += async (sender, e) => await InitializeConfigsAsync();
        }

        private async Task InitializeConfigsAsync()
        {
            _availableConfigs = await _configService.AvailableConfigs();
        }

        private async void LoadConfigButton_Clicked(object sender, EventArgs e)
        {
            List<string> availableConfigs = await _configService.AvailableConfigs();
            List<string> selectedConfig = await UserDialogHelper.GetSelectionFromUser(items: availableConfigs, prompt: "Select Config",
                selectionMode: SelectionMode.Multiple);

            foreach (string configName in selectedConfig)
            {
                var config = await _configService.LoadConfig(configName);
                if (config != null)
                {
                    AddConfigureConnectionButton(config);

                }
            }
        }

        private void AddConfigureConnectionButton(ServerConfig config)
        {
            // Check if the label for this config already exists
            bool configExists = _configsLayout.Children
                .OfType<StackLayout>() // Assuming each container is a StackLayout
                .SelectMany(stack => stack.Children)
                .OfType<Label>()
                .Any(label => label.Text == config.ServerName);

            if (configExists)
            {
                // Config already loaded, perhaps show a message or do nothing
                return;
            }

            // Create a label for the config name
            var configLabel = new Label
            {
                Text = config.ServerName,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };

            // Create the button for the config
            var configureServerButton = UIComponents.CreateButton("Configure Connection");
            configureServerButton.CommandParameter = config; 
            configureServerButton.Clicked += ConfigureServerButton_Clicked;

            _serverStatuses[config.ServerName] = new ServerStatus(config);
            var createServerButton = UIComponents.CreateButton("Create Server");
            createServerButton.CommandParameter = _serverStatuses[config.ServerName];
            createServerButton.Clicked += CreateServerButton_Clicked; 


            // Create a container for the label and button
            var container = new StackLayout
            {
                Children = { configLabel, configureServerButton, createServerButton }
            };

            // Add the container to the layout
            _configsLayout.Children.Add(container);
        }

        private async void ConfigureServerButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button configButton && configButton.CommandParameter is ServerConfig config)
            {
                // Navigate to SetUpConnectionPage, passing the Config instance

                await Navigation.PushAsync(new ConfigureConnectionPage(config));

            }

        }
        private async void CreateServerButton_Clicked(object sender, EventArgs e)
        {

            try
            {
                if (sender is Button button)
                {
                    var commandParameter = button.CommandParameter;
                    // Now you can check if the commandParameter is true or do whatever you need with it
                    if (commandParameter is ServerStatus serverStatus)
                    {
                        if (serverStatus.IsConnectedMQTT)
                        {
                            Console.WriteLine("Connected to MQTT and navigating");
                            ServerConnection serverConnection = new(serverStatus.Config);
                            Server server = new Server(serverConnection);
                            await Navigation.PushAsync(new ServerPage(server));
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


    }
}

