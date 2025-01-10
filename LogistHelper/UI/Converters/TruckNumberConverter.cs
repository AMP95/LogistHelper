using System.Globalization;
using System.Windows.Data;

namespace LogistHelper.UI.Converters
{
    public class TruckNumberConverter : IValueConverter
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
                string input = value.ToString().ToUpper();

                if ((input.Length == 2 || input.Length == 6) && input.Last() == ' ' || input.Length == 9 && input.Last() == '/')
                {
                    return input;
                }
                else
                {
                    input = string.Concat(input.ToUpper().Where(c => "АВЕКМНОРСУX0123456789".Contains(c)));
                    for (int i = 0; i < input.Length; i++)
                    {
                        if (i == 0 || i == 5 || i == 6)
                        {
                            if (char.IsDigit(input[i]))
                            {
                                input.Remove(i);
                            }
                        }
                        else
                        {
                            if (char.IsLetter(input[i]))
                            {
                                input.Remove(i);
                            }
                        }
                    }

                    if (input.Length == 1)
                    {
                        input += " ";
                    }
                    else if (input.Length > 1)
                    {
                        input = input.Insert(1, " ");
                    }

                    if (input.Length == 5)
                    {
                        input += " ";
                    }
                    else if (input.Length > 5)
                    {
                        input = input.Insert(5, " ");
                    }

                    if (input.Length == 8)
                    {
                        input += "/";
                    }
                    else if (input.Length > 8)
                    {
                        input = input.Insert(8, "/");
                    }

                    if (input.Length > 12)
                    {
                        input = input.Substring(0, 12);
                    }

                    return input;
                }
            }
        }
    }
}
