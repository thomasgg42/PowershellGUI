/*
 * Credit: Scott @Stackoverflow
 * https://stackoverflow.com/questions/397556/how-to-bind-radiobuttons-to-an-enum
 * 
 * view model?
 */

using System;
using System.Windows.Data;

namespace PowershellGUI
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
