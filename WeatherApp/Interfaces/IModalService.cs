namespace WeatherApp.Interfaces;

public interface IModalService
{
    Page? GetCurrentPage();
    Task<bool> ShowConfirmationAsync(string title, string message, string acceptText, string cancelText);
}