using Microsoft.Maui.Controls;

namespace DetectApp.Views.Server
{
    public class VideoStreamPage : ContentPage
    {
        private WebView videoWebView;

        public VideoStreamPage(string videoStreamUrl)  // Updated to accept a string parameter
        {
            // Initialize the WebView with fixed size
            videoWebView = new WebView
            {
                WidthRequest = 400,  // Set the fixed width
                HeightRequest = 600, // Set the fixed height
                VerticalOptions = LayoutOptions.Start, // Or CenterAndExpand, based on your layout needs
                HorizontalOptions = LayoutOptions.Start // Or CenterAndExpand, based on your layout needs
            };

            videoWebView.Source = new UrlWebViewSource
            {
                Url = videoStreamUrl  // Use the videoStreamUrl parameter here
            };

            // Set the WebView as the content of the page
            Content = videoWebView;
        }
    }
}
