using WeatherApp.Interfaces;

namespace WeatherApp.Models;

public class ModalService : IModalService
{
    public async Task<bool> ShowConfirmationAsync(string title, string message, string acceptText = "OK", string cancelText = "Cancel")
    {
        var page = Application.Current!.Windows.FirstOrDefault()?.Page;

        if (page != null)
        {
            return await page.DisplayAlert(title, message, acceptText, cancelText);
        }
        
        return false;
    }
}