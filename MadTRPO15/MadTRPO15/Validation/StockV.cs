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
    internal class StockV : ValidationRule
    {
       

        

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return null;

            string input = value.ToString().Trim().Replace('.', ',');

            if (string.IsNullOrEmpty(input))
            {
                return new ValidationResult(false, "Остаток не может быть пустой");
            }
            if (!int.TryParse(input, out int price))
            {
                return new ValidationResult(false, "Целое число");
            }
            if (price <= 0)
            {
                return new ValidationResult(false, "остаток не может быть отрицательной или равна нулю");
            }
            return ValidationResult.ValidResult;
            
        }
    }
}
