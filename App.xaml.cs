﻿using Chef.Interfaces;
using Chef.Models;
using Chef.Models.Database;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Chef
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>  
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDatabaseDriver, DatabaseDriver>();
            services.AddScoped<DatabaseContext, DatabaseContext>();
            services.AddSingleton<MainWindow>();
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
