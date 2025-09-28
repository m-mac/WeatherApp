using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using WeatherApp.Interfaces;
using WeatherApp.Models;

namespace WeatherApp.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private ILogger logger;
    private IModalService modalService;
    private IDatabaseProvider databaseProvider;
    private readonly ApiKeyViewModel apiKeyViewModel;

    [ObservableProperty] private string? apiKeyText;

    public SettingsViewModel(ILogger<SettingsViewModel> logger, 
                             ApiKeyViewModel apiKeyViewModel, 
                             IModalService modalService,
                             IDatabaseProvider databaseProvider)
    {
        this.logger = logger;
        this.databaseProvider = databaseProvider;
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

        await databaseProvider.DropAllAsync();

        apiKeyViewModel.DeleteApiKey(Constants.ApiKeyStorageKey);

        // Notifies the HomeViewModel to clear the list of locations
        WeakReferenceMessenger.Default.Send(new DataResetMessage());
        
        logger.LogInformation("Application data deleted");
    }
}