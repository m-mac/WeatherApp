namespace WeatherApp.Interfaces;

public interface IRepository<T> where T : class
{
    public T GetById(int id);
    public IEnumerable<T> GetAll();
    public void Add(T item);
}