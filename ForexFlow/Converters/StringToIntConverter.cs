using System.Globalization;
using System.Windows.Data;

namespace ForexFlow.View.Converters
{
    public class StringToIntConverter : IValueConverter
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

            if (int.TryParse(input, NumberStyles.Integer, culture, out int result))
            {
                return result;
            }

            return Binding.DoNothing;
        }
    }
}

