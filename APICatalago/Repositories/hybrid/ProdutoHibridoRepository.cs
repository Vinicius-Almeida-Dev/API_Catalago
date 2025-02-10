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

        public IEnumerable<Produto> GetProdutosPorCategoria(int id, Parameters parameters)
        {
            return GetAll()
                .Skip((parameters.pageNumber - 1) * parameters.pageSize)
                .Take(parameters.pageSize)
                .Where(p => p.CategoriaId == id);
        }

        public PagedList<Produto> GetProdutos(Parameters parameters)
        {
           var produtos = GetAll()
            .OrderBy(p => p.CategoriaId)
            .AsQueryable();

            return PagedList<Produto>.ToPagedList(produtos, parameters.pageNumber, parameters.pageSize);
        }

        public PagedList<Produto> GetProdutosFiltroPreco(ParametersProdutosFiltoPreco produtosFiltroParams)
        {
            var produtos = GetAll().AsQueryable();
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
            var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos, produtosFiltroParams.pageNumber,
                                                                                                  produtosFiltroParams.pageSize);
            return produtosFiltrados;
        }
    }
}
