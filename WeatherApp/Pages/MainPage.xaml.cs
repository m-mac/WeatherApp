using WeatherApp.ViewModels;

namespace WeatherApp.Pages;

public partial class MainPage
{
	public MainPage(MainViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();
	}
}
