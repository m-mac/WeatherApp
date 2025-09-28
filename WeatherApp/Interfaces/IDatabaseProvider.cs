using SQLite;
using SQLitePCL;
using WeatherApp.Models;

namespace WeatherApp.Interfaces;

public interface IDatabaseProvider
{
    SQLiteAsyncConnection GetConnection();
    Task InitAsync();
    Task<bool> HealthCheckAsync();
}