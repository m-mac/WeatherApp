namespace WeatherApp.Interfaces;

public interface IPreferencesService
{
    bool IsFirstRun { get; set; }
    DateTime LastSync { get; set; }
    
}