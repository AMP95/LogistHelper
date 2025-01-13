using System.Globalization;
using System.Windows.Controls;

namespace LogistHelper.UI.ValidationRules
{
    public class TimeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return ValidationResult.ValidResult;
        }
    }
}
