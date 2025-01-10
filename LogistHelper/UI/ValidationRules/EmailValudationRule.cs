using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace LogistHelper.UI.ValidationRules
{
    internal class EmailValudationRule : ValidationRule
    {
        private Regex _mail = new Regex("^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "Обязательно для заполнения");
            }
            else
            {
                string input = value.ToString();
                if (_mail.IsMatch(input))
                {
                    return ValidationResult.ValidResult;
                }
                else 
                {
                    return new ValidationResult(false, "Неверный формат электронной почты");
                }
            }
        }
    }
}
