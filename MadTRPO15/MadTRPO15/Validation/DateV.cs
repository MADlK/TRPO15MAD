using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MadTRPO15.Validation
{
    public class DateV : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(value == null)
            {
                return new ValidationResult(false, "Выберите дату");
            }

            if (value is DateTime dateTime)
            {
                if(dateTime == DateTime.MinValue)
                {
                    return new ValidationResult(false, "Укажите корректную дату");
                }
                
            }
            else if(string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(false, "Выберите дату");
            }
            return ValidationResult.ValidResult;
        }
    }
}
