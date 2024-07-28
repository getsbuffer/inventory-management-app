using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace IM.MAUI
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }

        public App()
        {
            InitializeComponent();

            var mauiApp = MauiProgram.CreateMauiApp();
            Services = mauiApp.Services;

            MainPage = new AppShell();
        }
    }
}
