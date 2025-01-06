using System.Globalization;
using System.Windows.Controls;

namespace LogistHelper.UI.ValidationRules
{
    internal class NumberValidationRule : ValidationRule
    {
        public double Min { get; set; }
        public double Max { get; set; } = double.MaxValue;

        public bool IsRequired { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                if (!IsRequired)
                {
                    return new ValidationResult(false, "Обязательно для заполнения");
                }
                else
                {
                    return ValidationResult.ValidResult;
                }
            }
            else 
            {
                if (double.TryParse(value.ToString(), CultureInfo.CurrentCulture, out double val))
                {
                    if (val <= Min)
                    {
                        return new ValidationResult(false, $"Значение должно быть больше {Min}");
                    }
                    else if (val >= Max)
                    {
                        return new ValidationResult(false, $"Значение должно быть меньше {Max}");
                    }
                    else
                    {
                        return ValidationResult.ValidResult;
                    }
                }
                else 
                {
                    return new ValidationResult(false, "Неверный формат числа");
                }
            }
            
        }
    }
}
