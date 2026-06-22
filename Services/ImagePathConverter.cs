using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace shoppingTechCart.Services
{
    public class ImagePathConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string? relativePath = value as string;
            string defaultImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", "default_product.png");

            if (string.IsNullOrWhiteSpace(relativePath))
            {
                if (File.Exists(defaultImagePath))
                {
                    try
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.UriSource = new Uri(defaultImagePath);
                        bitmap.EndInit();
                        return bitmap;
                    }
                    catch
                    {
                        return null;
                    }
                }
                return null;
            }

            // Clean the relative path
            string cleanPath = relativePath.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar);
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cleanPath);

            if (File.Exists(fullPath))
            {
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(fullPath);
                    bitmap.EndInit();
                    return bitmap;
                }
                catch
                {
                    // Fallback to default if load fails
                }
            }

            // Fallback to default_product.png if specific image not found
            if (File.Exists(defaultImagePath))
            {
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(defaultImagePath);
                    bitmap.EndInit();
                    return bitmap;
                }
                catch
                {
                    // Ignore
                }
            }

            return null;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
