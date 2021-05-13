using System;
using System.Globalization;
using System.Windows.Data;

namespace Chef.Shared.Converters
{
    public class Substring: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = value.ToString();
            if (val.Length >= 200)
            {
                val = val.Substring(0, 200) + "...";
            }
            return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
