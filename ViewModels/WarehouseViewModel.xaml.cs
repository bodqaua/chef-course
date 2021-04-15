﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using Chef.Interfaces;
using Chef.Models;
using Chef.Models.Database;
using Chef.ViewModels;

namespace Chef.ViewModels
{
    public partial class WarehouseViewModel : Page
    {
        private DatabaseContext databaseContext;

        public WarehouseViewModel(DatabaseContext databaseContext,
                             ViewModelFactory viewModelFactory)
        {
            InitializeComponent();
            this.databaseContext = databaseContext;
            this.loadProducts();
        }
        private void loadProducts()
        {
            WarehouseDataGrid.ItemsSource = this.databaseContext.Products.ToList<Product>(); ;
        }
    }
}