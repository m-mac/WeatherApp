using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using WeatherApp.Interfaces;

namespace WeatherApp.ViewModels;

public class AppShellViewModel : ObservableObject
{
	private IPreferencesService preferencesService;
	private ILogger<AppShellViewModel> logger;
	private IDatabaseProvider databaseProvider;
	
	public AppShellViewModel(IPreferencesService preferencesService, ILogger<AppShellViewModel> logger,
		IDatabaseProvider databaseProvider)
	{
		this.preferencesService = preferencesService;
		this.logger = logger;
		this.databaseProvider = databaseProvider;
	}
    
	// This must be async void so that it can be executed in OnAppearing without blocking
    public async void HandleFirstRunAsync()
    {
    	logger.LogInformation("Running startup tasks...");
    
    	if (preferencesService.IsFirstRun)
    	{
    		logger.LogInformation("Handling first run tasks...");
    		await databaseProvider.InitAsync();
    
    		preferencesService.IsFirstRun = false;
    	}
	    
	    if (!await databaseProvider.HealthCheckAsync())
	    {
		    await Application.Current.MainPage.DisplayAlert(
		    "Database Error", 
		    "The app could not initialize its local database. Some features may not work.", 
		    "OK");
	    }
    }
}