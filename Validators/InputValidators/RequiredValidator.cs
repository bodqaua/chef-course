using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chef.Validators.InputValidators
{
    public class RequiredValidator: AbstractValidator
    {
        public override bool checkControlValidity(object value)
        {
            MessageBox.Show(Convert.ToBoolean(value).ToString());
            return false;
        }
    }
}
