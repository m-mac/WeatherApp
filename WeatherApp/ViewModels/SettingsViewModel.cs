using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using WeatherApp.Interfaces;

namespace WeatherApp.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private ILogger logger;
    private IPreferencesService preferencesService;
    private IModalService modalService;

    private readonly ApiKeyViewModel apiKeyViewModel;

    [ObservableProperty] private string? apiKeyText;

    public SettingsViewModel(ILogger<SettingsViewModel> logger, IPreferencesService preferencesService,
        ApiKeyViewModel apiKeyViewModel, IModalService modalService)
    {
        this.logger = logger;
        this.preferencesService = preferencesService;
        this.modalService = modalService;
        this.apiKeyViewModel = apiKeyViewModel;
    }

    public bool HasApiKey => apiKeyViewModel.HasApiKey;

    [RelayCommand]
    private async Task OpenUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return;

        // Should open the default web view 
        await Launcher.OpenAsync(url);
    }

    [RelayCommand]
    private async Task SetApiKey(string? apiKey)
    {
        var confirm = await modalService.ShowConfirmationAsync(
            "Set API Key?",
            "This will overwrite an existing API key.",
            "SET",
            "Cancel"
        );

        if (!confirm) return;

        await apiKeyViewModel.SaveApiKeyAsync(apiKey);
        logger.LogInformation("API key Set");
    }


    [RelayCommand]
    private async Task ResetApp()
    {
        var confirm = await modalService.ShowConfirmationAsync(
            "Reset Application Data",
            "Are you sure you want to delete the application data?",
            "DELETE",
            "Cancel");

        if (!confirm) return;
        logger.LogInformation("Deleting application data");
        // Drop and recreate SQLite DB
        // Remove the API Key
        // Default anything in stored Preferences
        // Trigger recreate
    }
}