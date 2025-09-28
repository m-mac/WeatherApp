using WeatherApp.ViewModels;

namespace WeatherApp.Pages;

public partial class HomePage
{
	public HomePage(HomeViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();
	}
}
