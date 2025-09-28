using SQLite;
using WeatherApp.Interfaces;

namespace WeatherApp.Services;

public class Repository<T> : IRepository<T> where T : IHasId, new()
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

    public async Task<int> CountAsync()
    {
        return await Connection().Table<T>().CountAsync();
    }
    
    public async Task<bool> ExistsAsync(T item)
    {
        // Example: assume T has a unique Id or key property
        // Replace "Id" with the appropriate field
        var query = $"SELECT 1 FROM {typeof(T).Name} WHERE Id = ? LIMIT 1";
    
        var result = await Connection().FindWithQueryAsync<T>(query, item.Id);
        return result != null;
    }

    public async Task UpsertAsync(T item)
    {
        var existing = await ExistsAsync(item);

        if (existing)
        {
            await Connection().UpdateAsync(item);
        }
        else
        {
            await Connection().InsertAsync(item);
        }
    }

    public async Task<TEntity?> FirstOrDefaultAsync<TEntity>(
        Func<AsyncTableQuery<TEntity>, AsyncTableQuery<TEntity>> queryFunc)
        where TEntity : new()
    {
        var conn = Connection();
        var query = queryFunc(conn.Table<TEntity>());
        return await query.FirstOrDefaultAsync();
    }
}