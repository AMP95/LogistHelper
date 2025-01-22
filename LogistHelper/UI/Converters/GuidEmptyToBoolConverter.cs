using System.Globalization;
using System.Windows.Data;

namespace LogistHelper.UI.Converters
{
    public class GuidEmptyToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) 
            {
                return true;
            }
            if (Guid.TryParse(value.ToString(), out Guid guid)) 
            { 
                return guid == Guid.Empty;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
