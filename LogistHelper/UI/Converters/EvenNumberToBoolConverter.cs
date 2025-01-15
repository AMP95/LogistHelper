using System.Globalization;
using System.Windows.Data;

namespace LogistHelper.UI.Converters
{
    public class EvenNumberToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            else 
            {
                if (int.TryParse(value.ToString(), out int number)) 
                {
                    return number % 2 == 0;
                }
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
