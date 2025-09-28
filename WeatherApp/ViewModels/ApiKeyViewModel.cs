using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;

namespace WeatherApp.ViewModels;

// Must be partial for ObservableProperty to auto-generate properties
public partial class ApiKeyViewModel : ObservableObject
{
    private const string KeyName = "WeatherApiKey";
    private ILogger logger;
    private ISecureStorage secureStorage;

    [ObservableProperty] private string? apiKey;

    public bool HasApiKey => !string.IsNullOrEmpty(ApiKey);

    public ApiKeyViewModel(ILogger<ApiKeyViewModel> logger,
                           ISecureStorage secureStorage)
    {
        this.logger = logger;
        this.secureStorage = secureStorage;

        // Will initialise before long before it's used
        _ = LoadApiKeyAsync();
    }

    public async Task LoadApiKeyAsync()
    {
        var key = await secureStorage.GetAsync(KeyName);

        if (string.IsNullOrEmpty(key))
        {
            logger.LogInformation("No API key found");
            return;
        }

        this.ApiKey = key;

        OnPropertyChanged(nameof(HasApiKey));
    }

    public async Task SaveApiKeyAsync(string? apiKey)
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            logger.LogError("No API key provided to save");
            return;
        }

        await secureStorage.SetAsync(KeyName, apiKey.Trim());
        //logger.LogInformation("API key saved");

        OnPropertyChanged(nameof(HasApiKey));
    }

    public void DeleteApiKey(string? apiKey)
    {
        if (string.IsNullOrEmpty(apiKey)) return;

        secureStorage.Remove(apiKey);

        OnPropertyChanged(nameof(HasApiKey));
    }
}