using Microsoft.Maui.Controls;
namespace DetectApp
{
    public class UIComponents
    {
        public static Button CreateLoadConfigButton()
        {
            return new Button { Text = "Load Config" };
        }

        public static Button CreateDisconnectButton()
        {
            var disconnectButton = new Button
            {
                Text = "Disconnect",
                HeightRequest = 40  // Making this button smaller
            };
            return disconnectButton;
        }

        public static (Button ConnectToMQTTButton, Button TextReceiveButton, Button ImageReceiveButton) CreateMQTTAndServerButtons(ServerConfig config)
        {
            var connectToMQTTButton = new Button
            {
                Text = "Connect to MQTT",
                BackgroundColor = Colors.Blue,
                TextColor = Colors.White,
                Margin = new Thickness(5),
                CommandParameter = config

            };

            var textReceiveButton = new Button
            {
                Text = "Text Receive",
                BackgroundColor = Colors.Gray,
                TextColor = Colors.White,
                IsEnabled = false,
                Margin = new Thickness(5),
                CommandParameter = config

            };

            var imageReceiveButton = new Button
            {
                Text = "Image Receive",
                BackgroundColor = Colors.Gray,
                TextColor = Colors.White,
                IsEnabled = false,
                Margin = new Thickness(5),
                CommandParameter = config

            };

            return (connectToMQTTButton, textReceiveButton, imageReceiveButton);
        }

   


        public static Button CreateButton(string buttonText)
        {
            return new Button
            {
                Text = buttonText,
                BackgroundColor = Colors.BlueViolet, // You can set the default color you prefer
                TextColor = Colors.Black,
                Margin = new Thickness(5)
                // You can also set other properties like FontSize, BorderRadius, etc.
            };
        }
        public static Label CreateStatusLabel(string connectionType, Color statusColor)
        {
            return new Label
            {
                Text = connectionType,
                TextColor = statusColor,
            };
        }
    }
}