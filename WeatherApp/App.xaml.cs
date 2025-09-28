using WeatherApp.ViewModels;

namespace WeatherApp;

public partial class App : Application
{
	public static IServiceProvider Services { get; internal set; }
	
	public App(IServiceProvider services)
	{
		InitializeComponent();
		Services = services;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		// Capture the built IServicesProvider so that it can be accessed globally
		// this is required so that AppShell (the root of the app) can be provided
		// with a view model before the usual DI kicks off.
		var viewModel = Services.GetRequiredService<AppShellViewModel>();
		return new Window(new AppShell(viewModel));
	}
	
	public new static App Current => (App)Application.Current;
	
}