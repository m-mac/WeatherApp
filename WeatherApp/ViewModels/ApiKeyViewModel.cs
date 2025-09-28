using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;

namespace WeatherApp.ViewModels;

// Must be partial for ObservableProperty to auto-generate properties
public partial class ApiKeyViewModel :  ObservableObject
{
    private const string KeyName = "WeatherApiKey";
    private ILogger logger;

    [ObservableProperty] private string? apiKey;
    
    public bool HasApiKey => !string.IsNullOrEmpty(ApiKey);

    public ApiKeyViewModel(ILogger<ApiKeyViewModel> logger)
    {
        this.logger = logger;
    }
    
    public async Task LoadApiKeyAsync()
    {
        var key = await SecureStorage.Default.GetAsync(KeyName);

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
            logger.LogError("No API provided to save");
            return;
        }
        
        await SecureStorage.Default.SetAsync(KeyName, apiKey);
        logger.LogInformation("API key saved");
        
        OnPropertyChanged(nameof(HasApiKey));
    }
}