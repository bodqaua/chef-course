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
            this.redirect();
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
            this.databaseContext.Products.Add(Product.create("Tomato", "12.1", "12.1"));
            this.databaseContext.Products.Add(Product.create("Tomato", "12.1", "12.1"));
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
