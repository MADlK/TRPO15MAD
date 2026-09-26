using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MadTRPO15.Validation
{
    public class CBV : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(value == null)
            {
                return new ValidationResult(false, "выберите, путое нельзя");
            }

            if(int.TryParse(value.ToString(), out int id) && id <= 0)
            {
                return new ValidationResult(false, "выберите, путое нельзя");
            }

            return ValidationResult.ValidResult;
        }
    }
}
