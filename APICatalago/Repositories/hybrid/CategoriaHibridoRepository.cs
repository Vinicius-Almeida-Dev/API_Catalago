using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Repositories.Generic;
using APICatalago.Repositories.hybrid.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Repositories.hybrid
{
    public class CategoriaHibridoRepository : Repository<Categoria>, ICategoriaHibridoRepository
    {
        public CategoriaHibridoRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Categoria> GetCategoriasComProdutos()
        {
            return _context.Categorias.Include(c => c.Produtos).ToList();
        }
    }
}
