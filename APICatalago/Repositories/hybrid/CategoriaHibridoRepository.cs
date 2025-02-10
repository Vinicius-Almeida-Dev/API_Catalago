using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Pagination;
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
        public PagedList<Categoria> GetCategoriasFiltroNome(ParametersCategoriasFiltroNome categoriasParams)
        {
            var categorias = GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(categoriasParams.Nome)) 
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriasParams.Nome));
            }

            var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categorias,
                categoriasParams.pageNumber, categoriasParams.pageSize);

            return categoriasOrdenadas;
        }
    }
}
