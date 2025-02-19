using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories.Generic;
using APICatalago.Repositories.hybrid.Interfaces;

namespace APICatalago.Repositories.hybrid
{
    public class ProdutoHibridoRepository : Repository<Produto>, IProdutoHibridoRepository
    {
        public ProdutoHibridoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id, Parameters parameters)
        {
            var produtos = await GetAllAsync();
              
            var produtosPaginados = produtos
                .Skip((parameters.pageNumber - 1) * parameters.pageSize)
                .Take(parameters.pageSize)
                .Where(p => p.CategoriaId == id);

            return produtosPaginados;
        }

        public async Task<PagedList<Produto>> GetProdutosAsync(Parameters parameters)
        {
            var produtos = await GetAllAsync();
            var produtosOrdenados = produtos.OrderBy(p => p.CategoriaId).AsQueryable();

            return PagedList<Produto>.ToPagedList(produtosOrdenados, parameters.pageNumber, parameters.pageSize);
        }

        public async Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ParametersProdutosFiltoPreco produtosFiltroParams)
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
            var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos.AsQueryable(), produtosFiltroParams.pageNumber,
                                                                                                  produtosFiltroParams.pageSize);
            return produtosFiltrados;
        }
    }
}
