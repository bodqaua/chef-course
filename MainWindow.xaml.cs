using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Chef.Models;
using Chef.Models.Database;
using Chef.Pages;
using Chef.ViewModels;

namespace Chef
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DatabaseContext databaseContext;
        private ViewModelFactory viewModelFactory;
        public MainWindow(DatabaseContext databaseContext,
                          ViewModelFactory viewModelFactory)
        {
            this.databaseContext = databaseContext;
            this.viewModelFactory = viewModelFactory;

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

        private void generateTestEntity()
        {
            this.databaseContext.Products.RemoveRange(this.databaseContext.Products);
            Product product1 = new Product { Name = "Tomato", Price = 12.1, Quantity = 12.1 };
            Product product2 = new Product { Name = "Cucumber", Price = 12.1, Quantity = 12.1 };
            product1.prepare();
            product2.prepare();
            this.databaseContext.Products.Add(product1);
            this.databaseContext.Products.Add(product2);
            this.databaseContext.SaveChanges();
            MessageBox.Show("Success");
        }

        private void createTestEntities_Click(object sender, RoutedEventArgs e)
        {
            this.generateTestEntity();
        }

        private void warehouse_Click(object sender, RoutedEventArgs e)
        {
            this.redirect();
        }

        private void redirect()
        {
            this.Content = new Frame { Content = this.viewModelFactory.createWarehousePage() };

        }
    }
}
