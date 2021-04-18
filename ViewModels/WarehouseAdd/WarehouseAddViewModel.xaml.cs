using Chef.Models;
using Chef.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Chef.Validators;
using Chef.Validators.InputValidators;
using System.Reflection;

namespace Chef.ViewModels.WarehouseAdd
{
    /// <summary>
    /// Interaction logic for WarehouseAddViewModel.xaml
    /// </summary>
    public partial class WarehouseAddViewModel : Page
    {
        private DatabaseContext databaseContext;
        private ViewModelFactory viewModelFactory;
        private List<Product> products = new List<Product>();
        private FormGroup formGroup = new FormGroup();
        private TextBox textbox;
        public WarehouseAddViewModel(DatabaseContext databaseContext,
                                     ViewModelFactory viewModelFactory)
        {
            this.databaseContext = databaseContext;
            this.viewModelFactory = viewModelFactory;
            InitializeComponent();
            this.DataContext = this.formGroup;
        }

        private void addHandler_Click(object sender, RoutedEventArgs e)
        {
            if(!this.IsFormValid(this.formGroup))
            {
                return;
            }

            MessageBox.Show("Form Valid");
        }

        private bool IsFormValid(FormGroup formGroup)
        {
            bool isValid = true;
            foreach (ITextBoxGroup control in formGroup.controls)
            {
                textbox = (TextBox)FindName(control.Name);
                textbox.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                if (!this.isValidatorHasError(textbox.Text, control.Validators))
                {
                    isValid = false;
                }
            };
            return isValid;
        }

        private bool isValidatorHasError(object value, List<AbstractValidator> validators)
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
    }

    public class FormGroup
    {
        public ITextBoxGroup Name { get; set; } = new TextBoxGroup
        {
            Name = "Name",
            Value = "",
            Validators = new List<AbstractValidator>()
            {
                new RequiredValidator(),
            }
        };

        public ITextBoxGroup Price { get; set; } = new TextBoxGroup
        {
            Name = "Price",
            Value = "",
            Validators = new List<AbstractValidator>()
            {
                new RequiredValidator(),
                new MinValidator(0)
            }
        };

        public ITextBoxGroup Quantity { get; set; } = new TextBoxGroup
        {
            Name = "Quantity",
            Value = "-1",
            Validators = new List<AbstractValidator>()
            {
                new RequiredValidator(),
                new MinValidator(0)
            }
        };

        public List<ITextBoxGroup> controls = new List<ITextBoxGroup>();

        public FormGroup()
        {
            this.controls.Add(this.Name);
            this.controls.Add(this.Price);
            this.controls.Add(this.Quantity);
        }
    }
}
