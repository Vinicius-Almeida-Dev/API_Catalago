using APICatalago.Pagination;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace APICatalago.Repositories.Generic.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsync (Expression<Func<T, bool>> predicate);
        T? Create(T entity);
        T? Update(T entity);
        T? Delete(T entity);
    }
}
