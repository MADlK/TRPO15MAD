using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace MadTRPO15.Validation
{
    public class PriceV :  ValidationRule

    {
        

        
        

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return null;
            }
            string input = value?.ToString()?.Trim() ?? "";

            if (string.IsNullOrEmpty(input))
            {
                return new ValidationResult(false, "Цена не может быть пустой");
            }
            input=input.Replace('.', ',');
            if (!double.TryParse(input, out double price))
            {
                return new ValidationResult(false, "Введите корректное число");

            }
            if (price <= 0)
            {
                return new ValidationResult(false, "Цена не может быть отрицательной или равна нулю");
            }
            return ValidationResult.ValidResult;
        }
    }
}
