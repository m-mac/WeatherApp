namespace WeatherApp;

public static class Constants
{
    public const string ApiKeyStorageKey = "OpenWeatherApiKey";
    public const string DatabaseFilename = "WeatherApp.db3";
    
    // Creates DB if it doesn't exist and enables multithreaded access
    public const SQLite.SQLiteOpenFlags Flags =
        SQLite.SQLiteOpenFlags.ReadWrite |
        SQLite.SQLiteOpenFlags.Create |
        SQLite.SQLiteOpenFlags.SharedCache;
    
    
    public static string DatabasePath => 
        Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
}