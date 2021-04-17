using Chef.Validators.InputValidators;
using System.Collections.Generic;

namespace Chef.Validators
{
    public interface ITextBoxGroup
    {
        string Name { get; set; }
        List<AbstractValidator> Validators { get; set; }
        object Value { get; set; }
    }
}