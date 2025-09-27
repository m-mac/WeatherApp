using CommunityToolkit.Mvvm.ComponentModel;

namespace WeatherApp.Pages;

public abstract class MvvmContentPage<T> : ContentPage where  T : ObservableObject
{
    protected MvvmContentPage(T viewModel)
    {
        BindingContext = viewModel;
    }
}