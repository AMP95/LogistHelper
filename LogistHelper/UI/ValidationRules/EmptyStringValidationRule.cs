using System.Globalization;
using System.Windows.Controls;

namespace LogistHelper.UI.ValidationRules
{
    internal class EmptyStringValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString())) 
            {
                return new ValidationResult(false, "Обязательно для заполнения");
            }
            return ValidationResult.ValidResult;
        }
    }
}
