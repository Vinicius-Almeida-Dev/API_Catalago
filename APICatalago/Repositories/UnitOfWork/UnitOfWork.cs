using APICatalago.Context;
using APICatalago.Repositories.hybrid;
using APICatalago.Repositories.hybrid.Interfaces;

namespace APICatalago.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IProdutoHibridoRepository _pRepository;
        private ICategoriaHibridoRepository _cRepository;
        public AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProdutoHibridoRepository? ProdutoHibridoRepository
        {
            get
            {
                return _pRepository = _pRepository ?? new ProdutoHibridoRepository(_context);
            }
        }

        public ICategoriaHibridoRepository? CategoriaHibridoRepository
        {
            get
            {
                return _cRepository = _cRepository ?? new CategoriaHibridoRepository(_context);
            }
        }
        public async Task CommitAsync()
        {
          await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
