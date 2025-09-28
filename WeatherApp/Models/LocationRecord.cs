using SQLite;

namespace WeatherApp.Models;

public class LocationRecord
{
    [PrimaryKey,  AutoIncrement]
    public int Id { get; set; }
    
    public required string Name { get; set; }
    public required string Address { get; set; }
    
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    public DateTime SavedAt { get; set; }
    
    /// <summary>
    /// Pins the current weather recording for this location to the home page.
    /// </summary>
    public bool Pinned { get; set; }
    
    
    [Ignore]
    public WeatherRecord? CurrentWeatherRecord { get; set; }
}