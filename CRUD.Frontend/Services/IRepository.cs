namespace CRUD.Frontend.Services;

public interface IRepository
{
    Task<T> GetAsync<T>(string url);

    Task<object> PostAsync<T>(string url, T entity);

    Task<object> PutAsync<T>(string url, T entity);
    
    Task<T> GetByIdAsync<T>(string url, int id);

    Task<object> DeleteAsync(string url);
}
