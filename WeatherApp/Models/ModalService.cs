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
        var page = GetCurrentPage();
        if (page == null) return false;

        if (page != null)
        {
            return await page.DisplayAlert(title, message, acceptText, cancelText);
        }
        
        return false;
    }
}