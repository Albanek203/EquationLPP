using System;
using System.Windows;
using EquationLPP.models;
using EquationLPP.View;
using EquationLPP.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace EquationLPP {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public static IServiceProvider ServiceProvider = null!;
        public App() {
            var servicesCollection = new ServiceCollection();
            ConfigServices(servicesCollection);
            ServiceProvider = servicesCollection.BuildServiceProvider();
        }
        private static void ConfigServices(IServiceCollection serviceProvider) {
            // View
            serviceProvider.AddTransient<MainWindow>();
            // ViewModel
            serviceProvider.AddTransient<MainViewModel>();
            // Models
            serviceProvider.AddTransient<SystemEquations>();
        }
        private void App_OnStartup(object sender, StartupEventArgs e) {
            ServiceProvider.GetService<MainWindow>()!.Show();
        }
    }
}