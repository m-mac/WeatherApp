using SQLite;
using SQLitePCL;
using WeatherApp.Models;

namespace WeatherApp.Interfaces;

public interface IDatabaseProvider
{
    SQLiteAsyncConnection GetConnection();
    Task InitAsync(SQLiteAsyncConnection? connection = null);
    Task<bool> HealthCheckAsync();
    Task DropAllAsync();
}