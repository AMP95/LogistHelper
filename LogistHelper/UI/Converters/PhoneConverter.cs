using System.Globalization;
using System.Windows.Data;

namespace LogistHelper.UI.Converters
{
    internal class PhoneConverter : IValueConverter
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

                if (input.Contains("+7(") && (input.Length == 13 && input[10] == '-' && input[6] == ')' ||
                                              input.Length == 10 && input[6] == ')' ||
                                              input.Length == 6))
                {
                    return input;
                }
                else if(input.Length == 3)
                {
                    return string.Empty;
                }
                else
                {
                    input = string.Concat(input.Replace("+7(", "").Where(c => Char.IsDigit(c)));

                    if (input.Length == 3)
                    {
                        input += ")";
                    }
                    else if (input.Length > 3)
                    {
                        input = input.Insert(3, ")");
                    }

                    if (input.Length == 7)
                    {
                        input += "-";
                    }
                    else if (input.Length > 7)
                    {
                        input = input.Insert(7, "-");
                    }

                    if (input.Length == 10)
                    {
                        input += "-";
                    }
                    else if (input.Length > 10)
                    {
                        input = input.Insert(10, "-");
                    }

                    if (input.Length > 13)
                    {
                        input = input.Substring(0, 13);
                    }

                    if (input.Length > 0) 
                    { 
                        input = "+7(" + input;
                    }


                    return input;
                }
            }
        }
    }
}
