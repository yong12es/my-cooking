using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace mycooking.Converters
{

    public class Base64ToBitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && value is string base64String)
            {
                try
                {
                    BitmapImage biSource = new BitmapImage();
                    var bytes = System.Convert.FromBase64String(base64String);
                    using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                    {
                        stream.WriteAsync(bytes.AsBuffer());
                        stream.Seek(0);
                        biSource.SetSource(stream);
                    }
                    return biSource;
                }
                catch (Exception)
                {
                 
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
