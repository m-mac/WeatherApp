using SQLite;

namespace WeatherApp.Models;

public class WeatherRecord
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [Indexed]
    public int LocationId { get; set; }
    
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    
    
    /// <summary>
    /// Description of the weather condition (e.g. "Clear", "Partly Cloudy")
    /// </summary>
    public string Condition { get; set; }
    
    
    public DateTime RecordTime { get; set; }
}