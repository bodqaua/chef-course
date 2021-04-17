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
        public WarehouseAddViewModel(DatabaseContext databaseContext,
                                     ViewModelFactory viewModelFactory)
        {
            this.databaseContext = databaseContext;
            this.viewModelFactory = viewModelFactory;
            InitializeComponent();
            this.DataContext = new FormGroup();
        }

        private void addHandler_Click(object sender, RoutedEventArgs e)
        {
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
                new MinValidator(0)
            }
        };
    }
}
