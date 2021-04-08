using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Chef.Models;
using Chef.Models.Database;

namespace Chef
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DatabaseContext databaseContext;
        public MainWindow(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Product> products = databaseContext.Products.ToList();
            foreach (Product product in products)
            {
                MessageBox.Show(product.Name);
            }
        }
    }
}
