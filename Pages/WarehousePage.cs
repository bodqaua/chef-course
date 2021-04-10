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
using Chef.Models;
using Chef.Models.Database;

namespace Chef.Pages
{
    /// <summary>
    /// Interaction logic for Warehouse.xaml
    /// </summary>

    public partial class WarehousePage : Page, IWarehousePageModel
    {
        DatabaseContext databaseContext;
        List<Product> products;
        public WarehousePage(DatabaseContext databaseContext)
        {
            InitializeComponent();
            this.databaseContext = databaseContext;
            this.loadProducts();
        }

        private void generateDataGrid()
        {
            //WarehouseDataGrid.ItemsSource = ;
        }

        private void loadProducts()
        {
            this.products = this.databaseContext.Products.ToList<Product>();
            foreach(Product product in this.products)
            {
                MessageBox.Show(product.Name);
            }
        }
    }
}
