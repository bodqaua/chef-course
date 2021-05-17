using System.Windows;
using System.Windows.Controls;
using Chef.Models.Database;
using Chef.ViewModels;

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
