using WeatherApp.Interfaces;

namespace WeatherApp.Services;

public class GeocodingService : IGeocodingService
{
    /// <summary>
    /// Gets the first location for a given address.
    /// </summary>
    public async Task<IEnumerable<Location>> GetLocationsAsync(string address)
    {
        return await Geocoding.Default.GetLocationsAsync(address);
    }

    /// <summary>
    /// Gets the first placemark for a given location.
    /// </summary>
    public async Task<IEnumerable<Placemark>> GetPlacemarksAsync(Location location)
    {
        return await Geocoding.Default.GetPlacemarksAsync(location);
    }

    /// <summary>
    /// Optional helper: formats a placemark into a suggestion string.
    /// </summary>
    public string FormatSuggestion(Placemark placemark)
    {
        return $"{placemark.Locality}, {placemark.AdminArea}, {placemark.CountryName}";
    }
}

