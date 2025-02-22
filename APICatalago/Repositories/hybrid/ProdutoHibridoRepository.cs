using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories.Generic;
using APICatalago.Repositories.hybrid.Interfaces;
using X.PagedList;

namespace APICatalago.Repositories.hybrid
{
    public class ProdutoHibridoRepository : Repository<Produto>, IProdutoHibridoRepository
    {
        public ProdutoHibridoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IPagedList<Produto>> GetProdutosPorCategoriaAsync(int id, Parameters parameters)
        {
            var produtos = await GetAllAsync();
              
            var produtosPaginados = produtos.AsQueryable().Where(p => p.CategoriaId == id).ToPagedListAsync( parameters.pageNumber, parameters.pageSize);

            return await produtosPaginados;
        }

        public async Task<IPagedList<Produto>> GetProdutosAsync(Parameters parameters)
        {
            var produtos = await GetAllAsync();
            var produtosOrdenados = produtos.OrderBy(p => p.CategoriaId).AsQueryable();

            return await produtosOrdenados.ToPagedListAsync(parameters.pageNumber, parameters.pageSize);
        }

        public async Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ParametersProdutosFiltoPreco produtosFiltroParams)
        {
            var produtos = await GetAllAsync();
            if (produtosFiltroParams.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroParams.PrecoCriterio))
            {
                if (produtosFiltroParams.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco > produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtosFiltroParams.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco < produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtosFiltroParams.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco == produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
                }
            }
            var produtosFiltrados = await produtos.AsQueryable().ToPagedListAsync( produtosFiltroParams.pageNumber, produtosFiltroParams.pageSize);
            return produtosFiltrados;
        }
    }
}
