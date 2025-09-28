using WeatherApp.Interfaces;

namespace WeatherApp.Models;

public class ModalService : IModalService
{
    public Page? GetCurrentPage()
    {
        return Application.Current?
            .Windows
            .FirstOrDefault()?
            .Page;
    }
    public async Task<bool> ShowConfirmationAsync(string title, string message, string acceptText, string? cancelText)
    {
        var currentPage = GetCurrentPage();
        if (currentPage == null) return false;

        if (cancelText != null)
        {
            return await currentPage.DisplayAlert(title, message, acceptText, cancelText);
        }
        
        return await currentPage.DisplayAlert(title, message, acceptText, "OK");
    }

    public async Task PushModalAsync(Page page)
    {
        var currentPage = GetCurrentPage();
        if (currentPage == null) return;

        await currentPage.Navigation.PushModalAsync(page);
    }

    public async Task<string?> ShowActionSheetAsync(string title, string cancel, string? destruction, params string[] buttons)
    {
        var currentPage = GetCurrentPage();
        if (currentPage == null) return null;

        return await currentPage.DisplayActionSheet(title, cancel, destruction, buttons);
    }
    
    public async Task PopModalAsync()
    {
        var currentPage = GetCurrentPage();
        if (currentPage == null) return;
        await currentPage.Navigation.PopModalAsync();
    }
}