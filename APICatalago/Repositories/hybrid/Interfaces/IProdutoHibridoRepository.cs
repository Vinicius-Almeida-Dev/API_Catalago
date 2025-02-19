using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories.Generic.Interface;
using System.Linq.Expressions;

namespace APICatalago.Repositories.hybrid.Interfaces
{
    public interface IProdutoHibridoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id, Parameters parameters);
        Task<PagedList<Produto>> GetProdutosAsync(Parameters parameters);
        Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ParametersProdutosFiltoPreco produtosFiltoPreco);
    }
}
