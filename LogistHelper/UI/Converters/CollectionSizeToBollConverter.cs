using System.Globalization;
using System.Windows.Data;

namespace LogistHelper.UI.Converters
{
    internal class CollectionSizeToBollConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) 
            {
                return false;
            }
            if (value is IEnumerable<object> collection) 
            {
                return collection.Any();
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
