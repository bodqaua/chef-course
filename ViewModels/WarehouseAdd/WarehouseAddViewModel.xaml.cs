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
using Chef.Shared;
using Chef.Interfaces;

namespace Chef.ViewModels.WarehouseAdd
{
    /// <summary>
    /// Interaction logic for WarehouseAddViewModel.xaml
    /// </summary>
    public partial class WarehouseAddViewModel : AbstractPageController
    {
        private ProductService productService;
        private ViewModelFactory viewModelFactory;
        private ValidationController validationController;
        private List<Product> products = new List<Product>();
        private FormGroup formGroup = new FormGroup();
        public WarehouseAddViewModel(ValidationController validationController,
                                     ProductService productService,
                                     ViewModelFactory viewModelFactory)
        {
            this.productService = productService;
            this.viewModelFactory = viewModelFactory;
            this.validationController = validationController;
            this.Init(this);
            InitializeComponent();
            this.InitBackNavigation(this.viewModelFactory.createWarehousePage(), BackPanel);
            this.DataContext = this.formGroup;
        }

        private void addHandler_Click(object sender, RoutedEventArgs e)
        {
            if (!this.validationController.isFormValid(this, this.formGroup.controls))
            {
                return;
            }

            string name = this.Name.Text;
            string price = this.Price.Text;
            string quantity = this.Quantity.Text;
            this.addProduct(Product.create(name, price, quantity));
            this.updateProducts();
        }

        private void addProduct(Product product)
        {
            product.Id = this.products.Count;
            this.products.Add(product);
        }

        private void updateProducts()
        {
                this.WarehouseDataGrid.ItemsSource = null;
                this.WarehouseDataGrid.ItemsSource = this.products;
        }

        private void saveHandler_Click(object sender, RoutedEventArgs e)
        {
            this.productService.saveProductsUniq(this.products);
            this.WarehouseDataGrid.ItemsSource = null;
            MessageBox.Show("Продукты сохранены");
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
