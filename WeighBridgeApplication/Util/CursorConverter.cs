﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace WeighBridgeApplication.Util
{
    public class CursorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? "Wait" : "Arrow";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Arrow" : "Wait";
        }
    }
}