using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories.Generic;
using APICatalago.Repositories.hybrid.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace APICatalago.Repositories.hybrid
{
    public class CategoriaHibridoRepository : Repository<Categoria>, ICategoriaHibridoRepository
    {
        public CategoriaHibridoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasComProdutosAsync()
        {
            return await _context.Categorias.Include(c => c.Produtos).ToListAsync();
        }

        public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(ParametersCategoriasFiltroNome categoriasParams)
        {
            var categorias = await GetAllAsync();

            if (!string.IsNullOrEmpty(categoriasParams.Nome)) 
            {
                categorias = categorias.Where(c => c.Nome.IndexOf(categoriasParams.Nome, StringComparison.OrdinalIgnoreCase) >= 0)                ;
            }

            var categoriasOrdenadas = await categorias.ToPagedListAsync(categoriasParams.pageNumber, categoriasParams.pageSize);

            return categoriasOrdenadas;
        }
    }
}
