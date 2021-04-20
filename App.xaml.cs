using Chef.Interfaces;
using Chef.Models;
using Chef.Models.Database;
using Chef.Pages;
using Chef.Shared;
using Chef.ViewModels;
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
        private readonly IServiceProvider serviceProvider;
        public App()
        {
            var serviceCollection = new ServiceCollection();
            this.serviceProvider =  ConfigureServices(serviceCollection);
        }
        private IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<DatabaseContext>();
            services.AddScoped<ValidationController>();
            services.AddSingleton(p =>
            new ViewModelFactory(new DatabaseContext(), new ValidationController())
            );

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
