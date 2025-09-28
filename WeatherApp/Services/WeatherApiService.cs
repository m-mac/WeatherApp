using System.Globalization;
using Microsoft.Extensions.Logging;
using WeatherApp.Interfaces;
using WeatherApp.Models;
using WeatherApp.ViewModels;

namespace WeatherApp.Services;

public class WeatherApiService : IWeatherApiService
{
    private readonly ILogger<WeatherApiService> logger;
    private readonly HttpClient httpClient;
    private readonly ApiKeyViewModel apiKeyViewModel;

    public WeatherApiService(HttpClient httpClient,
                             ILogger<WeatherApiService> logger,
                             ApiKeyViewModel apiKeyViewModel
    )
    {
        this.httpClient = httpClient;
        this.logger = logger;
        this.apiKeyViewModel = apiKeyViewModel;
    }

    private string BuildRequestUrl(string lat, string lon, string apiKey, string units = "metric")
    {
        // Free tier current weather endpoint
        string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                     $"lat={lat}&lon={lon}&units={units}&appid={apiKey}";

        logger.LogInformation($"Built URL: {url}");
        return url;
    }


    public async Task<WeatherData?> GetWeatherResponseAsync(double latitude, double longitude, DateTime date)
    {
        var apiKey = apiKeyViewModel.ApiKey;
        if (apiKey == null)
        {
            logger.LogError("API key not found during request");
            return null;
        }

        var url = BuildRequestUrl(
            latitude.ToString(CultureInfo.InvariantCulture),
            longitude.ToString(CultureInfo.InvariantCulture),
            apiKey
        );

        var response = await httpClient.GetAsync(url);
        
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError($"Request failed with status code {response.StatusCode}");
            return null;
        }

        try
        {
            var json = await response.Content.ReadAsStringAsync();
            var weatherResponse = System.Text.Json.JsonSerializer.Deserialize<WeatherData>(json);

            return weatherResponse;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error reading response");
        }

        return null;
    }
}