using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace DetectApp.Views.Server
{
    public class LogsPage : ContentPage
    {
        private ListView _logListView;

        public LogsPage(ObservableCollection<string> recievedlogs)
        {
            Title = "Logs";

            _logListView = new ListView
            {
                ItemsSource = recievedlogs,
                ItemTemplate = new DataTemplate(() =>
                {
                    var textCell = new TextCell();
                    textCell.SetBinding(TextCell.TextProperty, ".");
                    return textCell;
                })
            };

            Content = new StackLayout
            {
                Children = { _logListView }
            };
        }
    }
}
