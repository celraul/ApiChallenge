namespace Cel.GameOfLife.Application.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T> GetById(string id);
    Task<T> InsertAsync(T entity);
    Task UpdateAsync(T entity);
}
