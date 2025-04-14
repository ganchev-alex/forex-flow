using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ForexFlow.View.Converters
{
    class StringToDecimalConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value as string;

            if (string.IsNullOrWhiteSpace(input))
            {
                return Binding.DoNothing;
            }

            if (decimal.TryParse(input, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, culture, out decimal result))
            {
                return result;
            }

            return Binding.DoNothing;
        }
    }
}
