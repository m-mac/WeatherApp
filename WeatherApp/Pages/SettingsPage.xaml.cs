using WeatherApp.ViewModels;

namespace WeatherApp.Pages;

public partial class SettingsPage
{
    public SettingsPage(SettingsViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }

    // EntryCell doesn't have a Command property, so I'm using the 'Completed' event
    private void ApiKeyEntryCell_OnCompleted(object? sender, EventArgs e)
    {
        if (!(BindingContext is SettingsViewModel viewModel)) return;
        if (!(sender is EntryCell entryCell)) return;
        var text = entryCell.Text;
    
        if (viewModel.SetApiKeyCommand.CanExecute(null))
            viewModel.SetApiKeyCommand.Execute(text);
        
        // Clear it after sending
        entryCell.Text = string.Empty;
    }
}