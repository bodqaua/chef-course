using Chef.Models;
using Chef.Models.Database;
using Chef.Shared;
using Chef.Validators;
using Chef.Validators.InputValidators;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Chef.ViewModels.WarehouseEdit
{
    /// <summary>
    /// Interaction logic for WarehouseEditViewModel.xaml
    /// </summary>
    public partial class WarehouseEditViewModel : Page
    {

        private ProductService productService;
        private ViewModelFactory viewModelFactory;
        private ValidationController validationController;
        private DbSet<Product> products;
        private FormGroup formGroup = new FormGroup();
        private int editProductId;
        public WarehouseEditViewModel(ValidationController validationController,
                                     ProductService productService,
                                     ViewModelFactory viewModelFactory)
        {
            this.productService = productService;
            this.viewModelFactory = viewModelFactory;
            this.validationController = validationController;
            this.DataContext = this.formGroup;
            InitializeComponent();
            this.initBindings();
            this.loadProducts();
        }

        public void EditProduct_Click(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = (DataGridRow)sender;
            if (!(row.DataContext is Product))
            {
                Name.Text = String.Empty;
                Price.Text = String.Empty;
                Quantity.Text = String.Empty;
                this.formGroup.Name.Value = String.Empty;
                this.formGroup.Price.Value = String.Empty;
                this.formGroup.Quantity.Value = String.Empty;
                this.formGroup.IsEnabled = false;
                return;
            }
            Product product = (Product)row.DataContext;
            this.editProductId = product.Id;
            Name.Text = product.Name;
            Price.Text = product.Price.ToString();
            Quantity.Text = product.Quantity.ToString();
            formGroup.IsEnabled = true;
        }

        private void loadProducts()
        {
            this.products = this.productService.loadProducts();
            WarehouseDataGrid.ItemsSource = this.products.ToList<Product>();
        }

        private void editHandler_Click(object sender, RoutedEventArgs e)
        {
            if (!this.validationController.isFormValid(this, this.formGroup.controls))
            {
                return;
            }
            Product product = Product.create(
                (string)this.formGroup.Name.Value,
                (string)this.formGroup.Price.Value,
                (string)this.formGroup.Quantity.Value
            );
            this.productService.updateProduct(this.editProductId, product);
            this.loadProducts();
        }

        private void initBindings()
        {
            Binding textBoxBinding = new Binding("IsEnabled");
            textBoxBinding.Source = this.formGroup;
            textBoxBinding.Mode = BindingMode.OneWay;
            BindingOperations.SetBinding(Name, TextBox.IsEnabledProperty, textBoxBinding);
            BindingOperations.SetBinding(Price, TextBox.IsEnabledProperty, textBoxBinding);
            BindingOperations.SetBinding(Quantity, TextBox.IsEnabledProperty, textBoxBinding);
        }

        public class FormGroup: INotifyPropertyChanged
        {
            public bool _IsEnabled = false;
            public bool IsEnabled
            {
                get { return _IsEnabled; }
                set
                {
                    if (_IsEnabled != value)
                    {
                        _IsEnabled = value;
                        OnPropertyChanged("IsEnabled");
                    }
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged([CallerMemberName] string property = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            }
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
                Value = "",
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

}
