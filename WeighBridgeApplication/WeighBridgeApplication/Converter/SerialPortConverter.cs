using System;
using System.Globalization;
using System.Windows.Data;

namespace WeighBridgeApplication.Converter
{
    public class SerialPortConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var format = parameter as string;
            if (!string.IsNullOrEmpty(format))
            {
                return string.Format(culture, format, value);
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}