using Chef.Interfaces;
using Chef.Models;
using Chef.Models.Database;
using Chef.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace Chef
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>  
    public partial class App : Application
    {
        private readonly ServiceProvider serviceProvider;
        public App()
        {
            var serviceCollection = new ServiceCollection();
            this.serviceProvider =  ConfigureServices(serviceCollection);
        }
        private ServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<DatabaseContext, DatabaseContext>();
            services.AddSingleton<IWarehousePageModel, WarehousePage>();

            services.AddScoped<MainWindow>();

            return services.BuildServiceProvider();
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = this.serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

    }
}
