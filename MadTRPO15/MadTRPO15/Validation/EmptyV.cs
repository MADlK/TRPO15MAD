using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MadTRPO15.Validation
{
    public class EmptyV : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value?.ToString()?.Trim() ?? "";

            if(string.IsNullOrEmpty(input))
            {
                return new ValidationResult(false, "Обязательное поле");
            }

            if (Regex.IsMatch(input, @"[@\\/#$%^&*()!?+=<>~|{}[\]]"))
            {
                return new ValidationResult(false, "Спецсимволы нельзя!");
            }



            return ValidationResult.ValidResult;
        }
    }
}
