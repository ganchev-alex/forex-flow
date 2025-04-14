using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace ForexFlow.View.Converters
{
    public class TransactionDirectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? "Add" : "Reduce";
            }
            return "Reduce"; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                return stringValue.Equals("Add", StringComparison.OrdinalIgnoreCase); 
            }
            return false;
        }
    }
}
