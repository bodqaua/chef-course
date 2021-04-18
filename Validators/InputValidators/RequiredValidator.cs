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
            return value.ToString().Length > 0;
        }

        public override string getMessage()
        {
            return "This field is required";
        }
    }
}
