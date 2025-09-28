using WeatherApp.ViewModels;

namespace WeatherApp.Pages;

public partial class AddLocationPage
{
    public AddLocationPage(AddLocationViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}