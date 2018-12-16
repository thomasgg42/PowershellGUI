using System;
using System.Windows.Data;

namespace PSGUI.Converters
    {
    /// <summary>
    /// Converts RadioButton selections to Enum values.
    /// See https://www.wpftutorial.net/valueconverters.html 
    /// </summary>
    public class ComparisonConverter : IValueConverter
        {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
            return value?.Equals(parameter);
            }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
            return value?.Equals(true) == true ? parameter : Binding.DoNothing;
            }
        }
    }
