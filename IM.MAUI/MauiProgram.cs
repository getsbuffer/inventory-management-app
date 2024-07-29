using IM.Library.Services;
using IM.MAUI.ViewModels;

namespace IM.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddSingleton<ShopItemService>();
            builder.Services.AddSingleton(ShoppingCartProxy.Instance);
            builder.Services.AddTransient<InventoryManagementViewModel>();

            return builder.Build();
        }
    }
}
