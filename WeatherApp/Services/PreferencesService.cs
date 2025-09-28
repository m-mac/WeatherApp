using Microsoft.Extensions.Logging;
using WeatherApp.Interfaces;

namespace WeatherApp.Services;


// Should rename to PreferencesService
public class PreferencesService(ILogger<PreferencesService> logger) : IPreferencesService
{

    public ILogger<PreferencesService> Logger { get; } = logger;
    
    
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

    
    /// <summary>
    /// Gets the time of the last successful weather data synchronization.
    /// </summary>
    /// <returns>
    /// The UTC time of the last completed sync. Returns <see cref="System.DateTime.MinValue"/> 
    /// if the application has never successfully synced weather data.
    /// </returns>
    public DateTime LastSync
    {
        get
        {
            long lastSync = Preferences.Get("LastSync", 0L);
            Logger.LogInformation("LastSync: {LastSync}", lastSync);
            return new DateTime(lastSync);
        }
        set
        {
            Preferences.Set("LastSync", value.Ticks);
            Logger.LogInformation("LastSync set to: {LastSync}", value.Ticks);
        }
        
    }
}