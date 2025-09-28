using SQLite;
using WeatherApp.Interfaces;

namespace WeatherApp.Services;
public class Repository<T> : IRepository<T> where T : new()
{
    private readonly IDatabaseProvider databaseProvider;
    private SQLiteAsyncConnection Connection() => databaseProvider.GetConnection();

    public Repository(IDatabaseProvider databaseProvider)
    {
        this.databaseProvider = databaseProvider;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Connection().Table<T>().ToListAsync();
    }
    
    public async Task<T?> GetByIdAsync(int id)
    {
        // Assumes T has a primary key property called 'Id'
        return await Connection().FindAsync<T>(id);
    }

    public async Task<int> InsertAsync(T item)
    {
        return await Connection().InsertAsync(item);
    }

    public async Task<int> UpdateAsync(T item)
    {
        return await Connection().UpdateAsync(item);
    }

    public async Task<int> DeleteAsync(T item)
    {
        return await Connection().DeleteAsync(item);
    }
}
