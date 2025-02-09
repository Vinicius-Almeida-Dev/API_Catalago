using APICatalago.Context;
using APICatalago.Pagination;
using APICatalago.Repositories.Generic.Interface;
using System.Linq.Expressions;

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
            return _context
                .Set<T>()
                .ToList();
        }

        public T? Get(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }
        
        public T Create(T entity)
        {
            _context.Set<T>().Add(entity);
           
            return entity;  
        }

        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);
          
            return entity;
        }       

        public T Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
         
            return entity;
        }       
    }
}
