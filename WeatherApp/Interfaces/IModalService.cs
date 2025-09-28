namespace WeatherApp.Interfaces;

public interface IModalService
{
    Page? GetCurrentPage();
    Task<bool> ShowConfirmationAsync(string title, string message, string acceptText, string cancelText);
    Task<string?> ShowActionSheetAsync(string title, string cancel, string? destruction, params string[] buttons);
    Task PushModalAsync(Page page);
    Task PopModalAsync();

}