using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace Timeta
{
    public class TimeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var errorResult = new ValidationResult(false, "Please enter a positive integer");
            if (!(value is string raw)) return errorResult;
            if (string.IsNullOrEmpty(raw)) return errorResult;

            if (int.TryParse(raw, out int newTime))
            {
                if (newTime < 0) return errorResult;
                return ValidationResult.ValidResult;
            }
            else return errorResult;
        }
    }
}
