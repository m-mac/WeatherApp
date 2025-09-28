using WeatherApp.Models;

namespace WeatherApp.Interfaces;

public interface IWeatherApiService
{
    Task<WeatherData?> GetWeatherResponseAsync(double latitude, double longitude, DateTime date);
}