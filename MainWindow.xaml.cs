using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Chef.Models;
using Chef.Models.Database;
using Chef.Models.Entities;
using Chef.Shared;
using Chef.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Chef
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ViewModelFactory viewModelFactory;
        public MainWindow(DatabaseContext databaseContext,
                          RecipeService recipeService,
                          ViewModelFactory viewModelFactory)
        {
            this.viewModelFactory = viewModelFactory;

            InitializeComponent();
            this.GoHome();
        }

        private void GoHome()
        {
            this.Content = new Frame { Content = this.viewModelFactory.createHomePage() };
        }
    }
}
