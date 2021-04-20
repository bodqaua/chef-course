using Chef.Interfaces;
using Chef.Validators;
using Chef.Validators.InputValidators;
using Chef.ViewModels.WarehouseAdd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Chef.Shared
{

    public class AbstractController: Window
    {
        public bool isValidatorHasError(object value, List<AbstractValidator> validators)
        {
            foreach (AbstractValidator validator in validators)
            {
                if (!validator.checkControlValidity(value))
                {
                    return false;
                }
            };
            return true;
        }

        public bool isFormValid(Page self, List<ITextBoxGroup> controls)
        {
            Dictionary<string, TextBox> elements = this.generetaTextBoxDictionary(self, controls);
            bool isValid = true;
            foreach (ITextBoxGroup control in controls)
            {
                TextBox textBox = elements[control.Name];
                textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                if (!this.isValidatorHasError(textBox.Text, control.Validators))
                {
                    isValid = false;
                }
            };
            return isValid;
        }

        public Dictionary<string, TextBox> generetaTextBoxDictionary(Page self, List<ITextBoxGroup> controls)
        {
            Dictionary<string, TextBox> elements = new Dictionary<string, TextBox>();
            foreach (ITextBoxGroup control in controls)
            {
                elements.Add(control.Name, (TextBox)self.FindName(control.Name));
            }

            return elements;
        }
    }
}
