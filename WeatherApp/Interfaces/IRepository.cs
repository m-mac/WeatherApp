namespace WeatherApp.Interfaces;

public interface IRepository<T> where T : new()
{ 
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<int> InsertAsync(T item);
    Task<int> UpdateAsync(T item);
    Task<int> DeleteAsync(T item);
}