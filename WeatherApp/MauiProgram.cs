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
        var app = builder.Build();

        App.Services = app.Services;
        
        return app;
    }

    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {        
        builder.Services.AddTransient<IDatabaseProvider, DatabaseProvider>();
        builder.Services.AddTransient<IPreferencesService, PreferencesService>();
        builder.Services.AddTransient<IModalService, ModalService>();
        
        // The DI container will magically resolve this for any T.
        builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        
        builder.Services.AddSingleton<ISecureStorage>(sp =>
        {
#if IOS
            if (DeviceInfo.DeviceType == DeviceType.Virtual)
            {
                // Simulator fallback. A provisioning profile is required f you create an Entitlements.plist to access
                // SecuredStorage. This swaps out the SecuredStorage for app preferences instead
                return new PreferencesStorage();
            }
#endif

            return SecureStorage.Default;
        });

        return builder;
    }

    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<HomeViewModel>();
        builder.Services.AddSingleton<SettingsViewModel>();
        builder.Services.AddSingleton<ApiKeyViewModel>();
        builder.Services.AddSingleton<AppShellViewModel>();
        builder.Services.AddSingleton<AddLocationViewModel>();

        return builder;
    }

    private static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<SettingsPage>();
        
        // This needs to be transient because it's used as a modal
        builder.Services.AddTransient<AddLocationPage>();

        return builder;
    }
}