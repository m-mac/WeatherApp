using Microsoft.Extensions.Logging;
using SQLite;
using WeatherApp.Interfaces;
using WeatherApp.Models;

namespace WeatherApp.Services;

public class DatabaseProvider : IDatabaseProvider
{
    private ILogger logger;
    
    public DatabaseProvider(ILogger<DatabaseProvider> logger)
    {
        this.logger = logger;
    }

    public async Task InitAsync()
    {
        var db = GetConnection();

        await db.CreateTableAsync<WeatherRecord>();
        await db.CreateTableAsync<LocationRecord>();
        
        logger.LogInformation("Created DB tables");
    }

    public SQLiteAsyncConnection GetConnection()
    {
        return new SQLiteAsyncConnection(Constants.DatabasePath);
    }
}