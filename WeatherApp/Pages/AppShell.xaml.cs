using WeatherApp.ViewModels;

namespace WeatherApp;

public partial class AppShell : Shell
{
	public AppShell(AppShellViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
	
	protected override void OnAppearing()
	{
		base.OnAppearing();

		if (BindingContext is AppShellViewModel viewModel)
		{
			// Initialize first-run tasks on appearing to avoid blocking the UI during construction
			viewModel.HandleFirstRunAsync();
		}
	}
}
