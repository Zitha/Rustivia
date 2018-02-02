using System;
using System.Globalization;
using System.Windows.Data;

namespace WeighBridgeApplication.Util
{
    [ValueConversion(typeof(Enum), typeof(bool))]
    public class EnumToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return false;
            }
            string enumValue = value.ToString();
            string targetValue = parameter.ToString();
            bool outPutValue = enumValue.EndsWith(targetValue, StringComparison.InvariantCultureIgnoreCase);
            return outPutValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return null;
            }
            bool useValue = (bool)value;
            string targetValue = parameter.ToString();
            if (useValue)
            {
                return Enum.Parse(targetType, targetValue);
            }
            return null;
        }
    }
}