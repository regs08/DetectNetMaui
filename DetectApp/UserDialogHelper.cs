using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetectApp
{
    public  class UserDialogHelper
    {
        public static async Task<List<T>> GetSelectionFromUser<T>(List<T> items, string prompt, SelectionMode selectionMode, T defaultItem = default)
        {
            var tcs = new TaskCompletionSource<List<T>>();

            var titleLabel = new Label
            {
                Text = prompt,
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            var collectionView = new CollectionView
            {
                SelectionMode = selectionMode,
                ItemsSource = items,
                ItemTemplate = new DataTemplate(() =>
                {
                    var label = new Label();
                    if (typeof(T).GetProperty("ServerName") != null)
                    {
                        label.SetBinding(Label.TextProperty, nameof(ServerConfig.ServerName));
                    }
                    else if (typeof(T) == typeof(string))
                    {
                        label.SetBinding(Label.TextProperty, ".");
                    }
                    return label;
                })
            };

            if (!EqualityComparer<T>.Default.Equals(defaultItem, default))
            {
                if (selectionMode == SelectionMode.Multiple)
                {
                    collectionView.SelectedItems.Add(defaultItem);
                }
                else
                {
                    collectionView.SelectedItem = defaultItem;
                }
            }

            var proceedButton = new Button { Text = "Proceed" };
            proceedButton.Clicked += (s, e) =>
            {
                var selectedItems = new List<T>();
                foreach (T selectedItem in collectionView.SelectedItems)
                {
                    selectedItems.Add(selectedItem);
                }

                tcs.SetResult(selectedItems);
            };

            var stackLayout = new StackLayout
            {
                Children = { titleLabel, collectionView, proceedButton }
            };

            var selectionPage = new ContentPage
            {
                Content = stackLayout
            };

            await Application.Current.MainPage.Navigation.PushAsync(selectionPage);

            var result = await tcs.Task;

            await Application.Current.MainPage.Navigation.PopAsync();

            return result;
        }


        public static async Task<double> GetDoubleFromUser(ContentPage page, string prompt, double defaultValue = .75)
        {
            // Exiting too soon not sure why
            double result = defaultValue;
            bool isValidInput = false;

            var titleLabel = new Label
            {
                Text = prompt,
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            var entry = new Entry
            {
                Text = defaultValue.ToString(),
                Keyboard = Keyboard.Numeric
            };

            var stackLayout = new StackLayout();
            stackLayout.Children.Add(titleLabel);
            stackLayout.Children.Add(entry);

            page.Content = stackLayout;

            var proceedButton = new Button { Text = "Proceed" };
            var tcs = new TaskCompletionSource<bool>();
            EventHandler handler = null;
            handler = async (s, e) =>
            {
                var inputText = entry.Text;
                if (double.TryParse(inputText, out var parsedValue) && parsedValue > 0.19)
                {
                    result = parsedValue;
                    isValidInput = true;

                    tcs.SetResult(true);
                    proceedButton.Clicked -= handler;  // Unsubscribe the handler
                    await page.Navigation.PopAsync();  // Navigate back to the previous page
                }
                else if (string.IsNullOrEmpty(inputText))
                {
                    // User canceled the prompt, exit the method
                    isValidInput = true;

                    tcs.SetResult(true);
                    proceedButton.Clicked -= handler;  // Unsubscribe the handler
                    await page.Navigation.PopAsync();  // Navigate back to the previous page
                }
                else
                {
                    await page.DisplayAlert(
                        "Invalid Input",
                        "Please enter a value greater than 0.19.",
                        "OK");
                }
            };

            proceedButton.Clicked += handler;

            stackLayout.Children.Add(proceedButton);  // Add the proceed button to the layout

            await tcs.Task;

            return isValidInput ? result: defaultValue;
        }


    }

}
