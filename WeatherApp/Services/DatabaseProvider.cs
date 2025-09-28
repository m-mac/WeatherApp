using Microsoft.Extensions.Logging;
using SQLite;
using WeatherApp.Interfaces;
using WeatherApp.Models;

namespace WeatherApp.Services;

public class DatabaseProvider : IDatabaseProvider
{
    private const string HealthCheckQuery = "SELECT count(*) FROM sqlite_master WHERE type='table'";

    private ILogger logger;

    public DatabaseProvider(ILogger<DatabaseProvider> logger)
    {
        this.logger = logger;
    }

    public SQLiteAsyncConnection GetConnection() => new SQLiteAsyncConnection(Constants.DatabasePath);

    public async Task InitAsync()
    {
        var db = GetConnection();

        await db.CreateTableAsync<WeatherRecord>();
        await db.CreateTableAsync<LocationRecord>();

        logger.LogInformation("Created DB tables");
    }
    
    /// <summary>
    /// Performs a basic health check against the SQLite database by attempting to connect and count the
    /// number of tables in the schema.
    /// </summary>
    public async Task<bool> HealthCheckAsync()
    {
        try
        {
            var connection = GetConnection();
            var result = await connection.ExecuteScalarAsync<int>(HealthCheckQuery);
            return result >= 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database health check failed");
            return false;
        }
    }
}