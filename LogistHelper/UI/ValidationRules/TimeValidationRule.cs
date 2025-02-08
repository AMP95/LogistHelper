using System.Globalization;
using System.Windows.Controls;

namespace LogistHelper.UI.ValidationRules
{
    public class TimeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "Обязательно для заполнения");
            }
            else
            {
                var val = value.ToString().ToUpper().Where(c => Char.IsLetterOrDigit(c));

                if (val.Take(5).Any(c => !"0123456789:".Contains(c)))
                {
                    return new ValidationResult(false, "Допустимы только цифры и двоеточие");
                }
                return ValidationResult.ValidResult;
            }
        }
    }
}
