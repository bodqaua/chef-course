using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chef.Validators.InputValidators
{
    public abstract class AbstractValidator
    {
        public abstract bool checkControlValidity(object value);
        public abstract string getMessage();
    }
}
