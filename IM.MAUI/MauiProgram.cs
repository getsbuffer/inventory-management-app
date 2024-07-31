using IM.Library.Services;
using IM.Library.Utilities;
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
            builder.Services.AddSingleton<WebRequestHandler>();
            builder.Services.AddSingleton<ShoppingCartProxy>();
            builder.Services.AddSingleton<SubscriptionService>();
            builder.Services.AddTransient<InventoryManagementViewModel>();
            builder.Services.AddTransient<CartViewModel>();
            builder.Services.AddTransient<SubscriptionViewModel>();

            return builder.Build();
        }
    }
}
