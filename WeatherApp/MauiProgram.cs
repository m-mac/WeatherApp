using MauiIcons.FontAwesome.Solid;
using Microsoft.Extensions.Logging;
using WeatherApp.Interfaces;
using WeatherApp.Models;
using WeatherApp.Pages;
using WeatherApp.Services;
using WeatherApp.ViewModels;

namespace WeatherApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .RegisterServices()
            .RegisterViewModels()
            .RegisterViews()
            .UseFontAwesomeSolidMauiIcons()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("fa-solid-900.ttf", "FontAwesomeSolid");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }


    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<ISettingsService, SettingsService>();
        builder.Services.AddTransient<IModalService, ModalService>();
        builder.Services.AddTransient<IDatabaseProvider, DatabaseProvider>();

        return builder;
    }

    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<HomeViewModel>();
        builder.Services.AddSingleton<SettingsViewModel>();
        builder.Services.AddSingleton<ApiKeyViewModel>();

        return builder;
    }

    private static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<SettingsPage>();

        return builder;
    }
}