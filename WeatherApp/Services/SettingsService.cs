using Microsoft.Extensions.Logging;
using WeatherApp.Interfaces;

namespace WeatherApp.Services;

public class SettingsService(ILogger<SettingsService> logger) : ISettingsService
{

    public ILogger<SettingsService> Logger { get; } = logger;
    
    
    public bool IsFirstRun
    {
        get
        {
            var isFirst = Preferences.Get("IsFirstRun", true);
            Logger.LogInformation("IsFirstRun: {IsFirstRun}", isFirst);
            return isFirst;
        }
        set
        {
            Preferences.Set("IsFirstRun", value);
            Logger.LogInformation("IsFirstRun set to: {IsFirstRun}", value);
        }
    }
}