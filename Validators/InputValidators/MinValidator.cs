using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chef.Validators.InputValidators
{
    class MinValidator : AbstractValidator
    {
        private int min;
        public MinValidator(int min)
        {
            this.min = min;
        }

        public override bool checkControlValidity(object value)
        {
            if (!Int32.TryParse(value.ToString(), out int res))
            {
                return false;
            }
            return Int32.Parse(value.ToString()) >= this.min;
        }

        public override string getMessage()
        {
            return "Number must be greater than " + this.min;
        }
    }
}
