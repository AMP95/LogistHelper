using LogistHelper.Models;
using System.Globalization;
using System.Windows.Data;

namespace LogistHelper.UI.Converters
{
    class EnumToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumVal) 
            {
                return enumVal.GetDescription();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
