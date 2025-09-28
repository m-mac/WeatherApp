using SQLite;
using WeatherApp.Interfaces;

namespace WeatherApp.Models;

public class LocationRecord : IHasId
{
    [PrimaryKey,  AutoIncrement]
    public int? Id { get; set; }
    
    public string Name { get; set; }
    public string Address { get; set; }
    
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    public DateTime SavedAt { get; set; }
    
    /// <summary>
    /// Pins the current weather recording for this location to the home page.
    /// </summary>
    public bool Pinned { get; set; }
    
    
    /// <summary>
    /// Gets the current weather record for this location. A refresh is triggered if it's missing or out of date. 
    /// </summary>
    [Ignore]
    public WeatherRecord? CurrentWeatherRecord { get; set; }
}