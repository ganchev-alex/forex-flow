using System;
using System.Globalization;
using System.Windows.Data;

namespace ForexFlow.View.Converters
{
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                return date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"{nameof(DateToStringConverter)} cannot convert back.");
        }
    }
}
