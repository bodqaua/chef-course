using Chef.Validators.InputValidators;
using System.Collections.Generic;

namespace Chef.Validators
{
    public class TextBoxGroup : ITextBoxGroup
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public List<AbstractValidator> Validators { get; set; }
    }
}
