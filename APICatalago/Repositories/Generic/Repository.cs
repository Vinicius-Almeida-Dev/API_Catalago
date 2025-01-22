using APICatalago.Context;
using APICatalago.Repositories.Generic.Interface;

namespace APICatalago.Repositories.Generic
{
    public class Repository<T> : IRepository<T> where T : class // esse where está indicando que o tipo T deve ser uma classe, como produto e categoria.
    {
        protected readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }


        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T? Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        
        public T Create(T entity)
        {
            throw new NotImplementedException();
        }

        public T Update(T entity)
        {
            throw new NotImplementedException();
        }       

        public T Delete(T entity)
        {
            throw new NotImplementedException();
        }       
    }
}
