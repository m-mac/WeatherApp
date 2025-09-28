using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeatherApp.Interfaces;

namespace WeatherApp.ViewModels;

public class SettingsViewModel : ObservableObject
{
    private ISettingsService settingsService;

    public IAsyncRelayCommand<string> OpenUrlCommand { get; }

    public SettingsViewModel(ISettingsService settingsService)
    {
        this.settingsService = settingsService;
        this.OpenUrlCommand = new AsyncRelayCommand<string>(ExecuteOpenUrlCommand);
    }

    private static async Task ExecuteOpenUrlCommand(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return;

        // Should open the default web view 
        await Launcher.OpenAsync(url);
    }
}