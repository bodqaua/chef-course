using Chef.Validators.InputValidators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Chef.Validators
{
    public class TextBoxValidator : ValidationRule
    {
        public string Type { get; set; }
        //public Wrapper Wrapper { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (Convert.ToBoolean(this.Type))
            {
                switch (Type)
                {
                    case "string":
                        return new ValidationResult(false, $"Error {this.Type}");
                    case "int":
                        bool isValid = Int32.TryParse(value.ToString(), out int res);
                        return this.getValidationResult(isValid, "incorrect number");
                    default:
                        return ValidationResult.ValidResult;
                }
            }

            return ValidationResult.ValidResult;
        }

        private ValidationResult getValidationResult(bool isValid, string message)
        {
            if (!isValid)
            {
                return new ValidationResult(false, message);
            }
            return ValidationResult.ValidResult;
        }
    }
}
