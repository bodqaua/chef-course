using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chef.Validators.InputValidators
{
    class MaxValidator : AbstractValidator
    {
        private int max;
        public MaxValidator(int max)
        {
            this.max = max;
        }

        public override bool checkControlValidity(object value)
        {
            if (!Int32.TryParse(value.ToString(), out int res))
            {
                return false;
            }
            return Int32.Parse(value.ToString()) <= this.max;
        }

        public override string getMessage()
        {
            return "Number must be less than " + this.max;
        }
    }
}
