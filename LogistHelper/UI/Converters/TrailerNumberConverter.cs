using System.Globalization;
using System.Windows.Data;

namespace LogistHelper.UI.Converters
{
    internal class TrailerNumberConverter : IValueConverter
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
                        if (i == 0 || i == 1)
                        {
                            if (char.IsDigit(input[i]))
                            {
                                input = input.Remove(i);
                            }
                        }
                        else
                        {
                            if (char.IsLetter(input[i]))
                            {
                                input = input.Remove(i);
                            }
                        }
                    }

                    if (input.Length == 2)
                    {
                        input += " ";
                    }
                    else if (input.Length > 2)
                    {
                        input = input.Insert(2, " ");
                    }

                    if (input.Length == 7)
                    {
                        input += "/";
                    }
                    else if (input.Length > 7)
                    {
                        input = input.Insert(7, "/");
                    }

                    if (input.Length > 10)
                    {
                        input = input.Substring(0, 10);
                    }

                    return input;
                }
            }
        }
    }
}
