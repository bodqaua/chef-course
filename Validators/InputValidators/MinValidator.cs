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
            if (!Double.TryParse(value.ToString(), out double res))
            {
                return false;
            }
            return Double.Parse(value.ToString()) >= this.min;
        }

        public override string getMessage()
        {
            return "Число должно быть не менее чем " + this.min;
        }
    }
}
