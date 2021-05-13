using Chef.Models.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Chef.Shared.Converters
{
    public class FirstImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<DbImage> images = (List<DbImage>)value;
            if (images.Count == 0)
            {
                return value;
            }
            byte[] binaryData = System.Convert.FromBase64String(images[0].Content);

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(binaryData);
            image.EndInit();
            return image;
;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
