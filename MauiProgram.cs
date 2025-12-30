using BTL_QLDiem_HSPTTH.Data.Services;
using BTL_QLDiem_HSPTTH.Services;
using BTL_QLDiem_HSPTTH.ViewModels;
using BTL_QLDiem_HSPTTH.Views.AdminV;
using BTL_QLDiem_HSPTTH.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace BTL_QLDiem_HSPTTH
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
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Poppins-Regular.ttf", "Poppins-Regular");
                    fonts.AddFont("Poppins-SemiBold.ttf", "Poppins-SemiBold");
                    fonts.AddFont("MaterialSymbolsRounded.ttf", "MaterialSymbolsRounded");
                })
                .UseMauiCommunityToolkit();

#if DEBUG
    		builder.Logging.AddDebug();

#endif
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<DatabaseService>();

            builder.Services.AddTransient<Login>();
            builder.Services.AddTransient<LoginVM>();
            builder.Services.AddSingleton<UserSessionService>();

            builder.Services.AddSingleton<QuanlyLop>();



            builder.Services.AddSingleton<ToastService>();
            return builder.Build();
        }
    }
}
