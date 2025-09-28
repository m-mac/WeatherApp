namespace WeatherApp.ViewModels;


public class PreferencesStorage : ISecureStorage
{
    public Task<string?> GetAsync(string key)
        => Task.FromResult(Preferences.Get(key, null));

    public Task SetAsync(string key, string value)
    {
        Preferences.Set(key, value);
        return Task.CompletedTask;
    }

    public bool Remove(string key)
    {
        try
        {
            Preferences.Remove(key);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            return false;
        }
        return true;
    }

    public void RemoveAll() => Preferences.Clear();
}