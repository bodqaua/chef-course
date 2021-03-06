using Chef.Validators.InputValidators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Chef.Validators
{
    public class Wrapper : DependencyObject
    {
        public static readonly DependencyProperty ValidatorProperty =
             DependencyProperty.Register("Validators", typeof(List<AbstractValidator>),
             typeof(Wrapper), new FrameworkPropertyMetadata(null));

        public List<AbstractValidator> Validators
        {
            get { return (List<AbstractValidator>)GetValue(ValidatorProperty); }
            set { SetValue(ValidatorProperty, value); }
        }
    }
    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new PropertyMetadata(null));
    }
    public class TextBoxValidator : ValidationRule
    {
        public string Type { get; set; }
        public Wrapper Wrapper { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            List<AbstractValidator> validators = Wrapper.Validators;
            bool isTypeValid = true;
            string invalidMessage = "";

            if (!string.IsNullOrEmpty(this.Type))
            {
                switch (this.Type)
                {
                    case "int":
                        isTypeValid = Int32.TryParse(value.ToString(), out int intRes);
                        invalidMessage = "Не правильное число (int)";
                        break;
                    case "double":
                        isTypeValid = Double.TryParse(value.ToString(), out double doubleRes);
                        invalidMessage = "Не правильное число (double)";
                        break;
                    default:
                        return this.getValidationResult(false, "Не правильный тип поля");
                }
            }

            if(!isTypeValid)
            {
                return this.getValidationResult(false, invalidMessage);
            }

            if (Convert.ToBoolean(validators.Count))
            {
                foreach (AbstractValidator validator in validators)
                {
                    bool isValid = validator.checkControlValidity(value);
                    if (!isValid)
                    {
                        return this.getValidationResult(isValid, validator.getMessage());
                    }
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
