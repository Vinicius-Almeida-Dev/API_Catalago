using APICatalago.Pagination;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace APICatalago.Repositories.Generic.Interface
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T? Get (Expression<Func<T, bool>> predicate);
        T Create(T entity);
        T Update(T entity);
        T Delete(T entity);
    }
}
