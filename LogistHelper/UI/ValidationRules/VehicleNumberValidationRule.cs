﻿using System.Globalization;
using System.Windows.Controls;

namespace LogistHelper.UI.ValidationRules
{
    internal class VehicleNumberValidationRule : ValidationRule
    {
        public int ValidLength { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "Обязательно для заполнения");
            }
            else
            {
                var val = value.ToString().ToUpper().Where(c => Char.IsLetterOrDigit(c));

                if (val.Take(ValidLength).Any(c => !"АВЕКМНОРСУX0123456789".Contains(c))) 
                {
                    return new ValidationResult(false, "Допустимы только буквы кириллицей и цифры");
                }
                return ValidationResult.ValidResult;
            }
        }
    }
}
