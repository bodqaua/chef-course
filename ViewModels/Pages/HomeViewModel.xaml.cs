using Chef.Shared;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Chef.ViewModels.Pages
{
    /// <summary>
    /// Interaction logic for HomeViewModel.xaml
    /// </summary>
    public partial class HomeViewModel : AbstractPageController
    {
        private readonly ViewModelFactory viewModelFactory;
        public HomeViewModel(ViewModelFactory viewModelFactory)
        {
            this.viewModelFactory = viewModelFactory;
            this.Init(this);
            InitializeComponent();
        }

        private void RedirectAction(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string name = Convert.ToString(button.Tag);
            MethodInfo method = viewModelFactory.GetType().GetMethod(name);
            if (method != null)
            {
                this.Redirect((Page)method.Invoke(this.viewModelFactory, null));
            }
        }
    }
}
