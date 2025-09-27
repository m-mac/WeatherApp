namespace WeatherApp.Interfaces;

public interface IGeocodingService
{
    // This might not make sense to create an interface for this. Not sure yet.
    public Task<Location> GetLocation(string address);
    public Task<Location> GetLocation(double latitude, double longitude);
}