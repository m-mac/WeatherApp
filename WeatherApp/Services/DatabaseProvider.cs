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

    public async Task InitAsync(SQLiteAsyncConnection? connection = null)
    {
        connection ??= GetConnection();

        await connection.CreateTableAsync<WeatherRecord>();
        await connection.CreateTableAsync<LocationRecord>();
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
            var tableCount = await connection.ExecuteScalarAsync<int>(HealthCheckQuery);
            if (tableCount == 0)
                return false;
            
            return true;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database health check failed");
            return false;
        }
    }

    public async Task DropAllAsync()
    {
        try
        {
            var connection = GetConnection(); // Should return SQLiteAsyncConnection

            // Drop tables
            await connection.DropTableAsync<LocationRecord>();
            await connection.DropTableAsync<WeatherRecord>();
            
            // Recreate
            await InitAsync(connection);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to drop tables");
            throw;
        }
    }
}