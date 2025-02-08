using System.Globalization;
using System.Windows.Data;

namespace LogistHelper.UI.Converters
{
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }
            else 
            {
                string input = value.ToString();

                if (input.Length == 3 && input.Last() == ':')
                {
                    return input.Substring(0, 2);
                }
                else
                {
                    input = string.Concat(input.ToUpper().Where(c => "0123456789".Contains(c)));
                    for (int i = 0; i < input.Length; i++)
                    {
                        if (i == 0)
                        {
                            if (input[i] != '0' && input[i] != '1' && input[i] != '2')
                            {
                                input = input.Remove(i);
                            }
                        }
                        else if (i == 1)
                        {
                            if (input[0] == '2' && (input[i] != '0' && input[i] != '1' && input[i] != '2' && input[i] != '3' && input[i] != '4'))
                            {
                                input = input.Remove(i);
                            }
                        }
                        else if (i == 2) 
                        {
                            if (input[i] != '0' && input[i] != '1' && input[i] != '2' && input[i] != '3' && input[i] != '4' && input[i] != '5')
                            {
                                input = input.Remove(i);
                            }
                        }
                    }

                    if (input.Length == 2)
                    {
                        input += ":";
                    }
                    else if (input.Length > 2)
                    {
                        input = input.Insert(2, ":");
                    }

                    if (input.Length > 5)
                    {
                        input = input.Substring(0, 5);
                    }

                    return input;
                }
            }
        }
    }
}
