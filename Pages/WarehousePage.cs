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

namespace Chef.Pages
{
    /// <summary>
    /// Interaction logic for Warehouse.xaml
    /// </summary>

    public partial class WarehousePage : Page
    {
        private DatabaseContext databaseContext;
        //private ViewModelFactory viewModelFactory;

        public DataTable dataTable { get; private set; }

        public WarehousePage(DatabaseContext databaseContext,
                             ViewModelFactory viewModelFactory)
        {
            InitializeComponent();
            this.databaseContext = databaseContext;
            //this.viewModelFactory = viewModelFactory;
            this.dataTable = new DataTable();
            this.loadProducts();
        }


        private void loadProducts()
        {
            WarehouseDataGrid.ItemsSource = this.databaseContext.Products.ToList<Product>(); ;
        }
    }
}