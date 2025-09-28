namespace WeatherApp.Interfaces;

public interface IModalService
{
    Task<bool> ShowConfirmationAsync(string title, string message, string acceptText, string cancelText);
}