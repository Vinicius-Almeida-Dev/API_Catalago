using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Repositories.Generic;
using APICatalago.Repositories.hybrid.Interfaces;

namespace APICatalago.Repositories.hybrid
{
    public class ProdutoHibridoRepository : Repository<Produto>, IProdutoHibridoRepository
    {
        public ProdutoHibridoRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Produto> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(p => p.CategoriaId == id);
        }
    }
}
