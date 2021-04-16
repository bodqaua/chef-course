using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Chef.Validators
{
    class TextBoxValidator : ValidationRule
    {
        public string Type { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return new ValidationResult(false, $"Error {this.Type}");
            //return ValidationResult.ValidResult;
        }
    }
}
