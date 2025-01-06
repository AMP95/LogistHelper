using DTOs;
using System.Globalization;
using System.Windows.Controls;

namespace LogistHelper.UI.ValidationRules
{
    internal class EmailValudationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "Обязательно для заполнения");
            }
            else
            {
                string error = ModelsValidator.IsMailValid(value.ToString());
                if (string.IsNullOrWhiteSpace(error))
                {
                    return ValidationResult.ValidResult;
                }
                else
                {
                    return new ValidationResult(false, error);
                }
            }
        }
    }
}
