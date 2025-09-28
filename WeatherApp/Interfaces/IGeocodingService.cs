namespace WeatherApp.Interfaces;

public interface IGeocodingService
{
    Task<IEnumerable<Location>> GetLocationsAsync(string address);
    Task<IEnumerable<Placemark>> GetPlacemarksAsync(Location location);
    string FormatSuggestion(Placemark placemark);
}