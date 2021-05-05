using Chef.Models;
using Chef.Models.Database;
using Chef.Shared;
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

namespace Chef.ViewModels.Recipe
{
    /// <summary>
    /// Interaction logic for RecipeAdd.xaml
    /// </summary>
    public partial class RecipeAdd : Page
    {
        private ValidationController validationController;
        private ProductService productService;
        private ViewModelFactory viewModelFactory;
        private List<Product> products;
        private List<Product> ingredients;
        public RecipeAdd(ValidationController validationController,
                                     ProductService productService,
                                     ViewModelFactory viewModelFactory)
        {
            this.validationController = validationController;
            this.productService = productService;
            this.viewModelFactory = viewModelFactory;

            InitializeComponent();
            this.getIngredients();
        }

        private void getIngredients()
        {
            this.products = this.productService.loadProducts().ToList();
            IngredientsComboBox.SelectedValuePath = "Key";
            IngredientsComboBox.DisplayMemberPath = "Value";
            foreach (Product product in this.products)
            {
                IngredientsComboBox.Items.Add(new KeyValuePair<int, string>(product.Id, product.Name));
            }
        }

        private void IngredientsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            KeyValuePair<int, string> item = (KeyValuePair<int, string>)IngredientsComboBox.SelectedItem;
            MessageBox.Show(item.Value.ToString());
        }
    }
}
