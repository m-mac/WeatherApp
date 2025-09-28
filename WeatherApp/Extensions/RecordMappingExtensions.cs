using WeatherApp.Models;

namespace WeatherApp.Extensions;

public static class RecordMappingExtensions
{
    
    public static WeatherRecord ToWeatherRecord(this WeatherData response, int? locationId)
    {
        if (response == null) throw new ArgumentNullException(nameof(response));

        return new WeatherRecord
        {
            LocationId = locationId,
            Temperature = response.Main?.Temp ?? 0,
            Humidity = response.Main?.Humidity ?? 0,
            Condition = response.Weather?.FirstOrDefault()?.Main ?? "Unknown",
            RecordTime = DateTimeOffset.FromUnixTimeSeconds(response.Dt).UtcDateTime
        };
    }

    public static LocationRecord ToLocationRecord(this Placemark placemark)
    {
        var location = placemark.Location;
        
        if (location == null) throw new ArgumentNullException(nameof(location));
        if (placemark == null) throw new ArgumentNullException(nameof(placemark));

        // Build a human-readable address string safely
        var addressParts = new[]
        {
            string.IsNullOrWhiteSpace(placemark.Locality) ? null : placemark.Locality,
            string.IsNullOrWhiteSpace(placemark.CountryName) ? null : placemark.CountryName
        };

        string address = string.Join(", ", addressParts.Where(p => !string.IsNullOrWhiteSpace(p)));

        // Use Locality or Thoroughfare as the Name, fallback to address if empty
        string name = !string.IsNullOrWhiteSpace(placemark.Locality)
            ? placemark.Locality
            : !string.IsNullOrWhiteSpace(placemark.Thoroughfare)
                ? placemark.Thoroughfare
                : address;

        return new LocationRecord
        {
            Name = name,
            Address = address,
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            SavedAt = DateTime.UtcNow,
            Pinned = false
        };
    }
}