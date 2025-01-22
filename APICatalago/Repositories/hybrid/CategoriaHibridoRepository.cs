using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Repositories.Generic;
using APICatalago.Repositories.hybrid.Interfaces;

namespace APICatalago.Repositories.hybrid
{
    public class CategoriaHibridoRepository : Repository<Categoria>, ICategoriaHibridoRepository
    {
        public CategoriaHibridoRepository(AppDbContext context) : base(context)
        {
        }
    }
}
