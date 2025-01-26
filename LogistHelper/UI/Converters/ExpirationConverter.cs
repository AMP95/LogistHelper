using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LogistHelper.UI.Converters
{
    internal class ExpirationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 1;
            }
            else 
            {
                if (int.TryParse(value.ToString(), out int days)) 
                {
                    if (days < 0)
                    {
                        return -1;
                    }
                    else if (days == 0)
                    {
                        return 0;
                    }
                    else 
                    {
                        return 1;
                    }
                }
            }
            return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
