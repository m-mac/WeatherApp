using Microsoft.Extensions.Logging;
using WeatherApp.Interfaces;

namespace WeatherApp.Services;

public class SettingsService(ILogger<SettingsService> logger) : ISettingsService
{

    public ILogger<SettingsService> Logger { get; } = logger;
    
    
    /// <summary>
    /// Gets or sets a value indicating whether the application is running for the first time on this device.
    /// </summary>
    /// <remarks>
    /// This property is typically read once on startup. Setting it to <c>false</c> prevents subsequent checks from returning <c>true</c>.
    /// </remarks>
    /// <value>
    /// <c>true</c> if this is the initial launch; otherwise, <c>false</c>.
    /// </value>
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