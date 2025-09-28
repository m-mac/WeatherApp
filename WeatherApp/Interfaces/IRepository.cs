using SQLite;

namespace WeatherApp.Interfaces;

public interface IRepository<T> where T : IHasId, new()
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<int> InsertAsync(T item);
    Task<int> UpdateAsync(T item);
    Task<int> DeleteAsync(T item);
    Task<int> CountAsync();
    Task<bool> ExistsAsync(T item);
    Task UpsertAsync(T item);

    Task<TEntity?> FirstOrDefaultAsync<TEntity>(Func<AsyncTableQuery<TEntity>, AsyncTableQuery<TEntity>> queryFunc)
        where TEntity : new();
}