using SQLite;
using SQLitePCL;
using WeatherApp.Models;

namespace WeatherApp.Interfaces;

public interface IDatabaseProvider
{
    Task InitAsync();
    SQLiteAsyncConnection GetConnection();
}