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
        }

        private void addHandler_Click(object sender, RoutedEventArgs e)
        {
            string name = Name.Text;
            double price = Convert.ToDouble(Price.Text);
            double quantity = Convert.ToDouble(Quantity.Text);
        }
    }
}
