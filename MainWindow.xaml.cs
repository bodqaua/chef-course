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
using System.Diagnostics;
using Chef.Models;
using Chef.Interfaces;

namespace Chef
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IDatabaseDriver databaseDriver;
        public MainWindow(IDatabaseDriver databaseDriver)
        {
            this.databaseDriver = databaseDriver;
            InitializeComponent();
            MessageBox.Show(this.databaseDriver.init());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
