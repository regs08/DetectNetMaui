using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace DetectApp
{
    public class PhotoViewerPage : ContentPage
    {
        private readonly ObservableCollection<ImageSource> _recentImages;

        public PhotoViewerPage(ObservableCollection<ImageSource> recentImages)
        {
            _recentImages = recentImages;

            var imageCollection = new CollectionView
            {
                ItemsSource = _recentImages,
                ItemTemplate = new DataTemplate(() =>
                {
                    var image = new Image
                    {
                        WidthRequest = 200, // Set the desired width
                        HeightRequest = 200, // Set the desired height
                        Aspect = Aspect.AspectFit // Adjust the aspect ratio as needed
                    };
                    image.SetBinding(Image.SourceProperty, ".");
                    return image;
                })
            };
            Content = imageCollection;
        }
    }
}
