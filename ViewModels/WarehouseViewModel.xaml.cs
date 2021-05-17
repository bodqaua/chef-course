using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Chef.Models;
using Chef.Models.Database;
using Chef.Shared;

namespace Chef.ViewModels
{
    public partial class WarehouseViewModel : AbstractPageController
    {
        private DatabaseContext databaseContext;
        private ViewModelFactory viewModelFactory;

        public WarehouseViewModel(DatabaseContext databaseContext,
                             ViewModelFactory viewModelFactory)
        {
            InitializeComponent();
            this.databaseContext = databaseContext;
            this.viewModelFactory = viewModelFactory;
            this.Init(this);
            this.InitBackNavigation(this.viewModelFactory.createHomePage(), BackPanel);
            this.loadProducts();
        }
        private void loadProducts()
        {
            WarehouseDataGrid.ItemsSource = this.databaseContext.Products.ToList<Product>(); ;
        }

        private void addStuffs_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Frame { Content = this.viewModelFactory.createWarehouseAddPage() };

        }

        private void editStuffs_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new Frame { Content = this.viewModelFactory.createWarehouseEditPage() };
        }
    }
}
