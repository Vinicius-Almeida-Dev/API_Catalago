using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories.Generic.Interface;
using System.Linq.Expressions;
using X.PagedList;

namespace APICatalago.Repositories.hybrid.Interfaces
{
    public interface IProdutoHibridoRepository : IRepository<Produto>
    {
        Task<IPagedList<Produto>> GetProdutosPorCategoriaAsync(int id, Parameters parameters);
        Task<IPagedList<Produto>> GetProdutosAsync(Parameters parameters);
        Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ParametersProdutosFiltoPreco produtosFiltoPreco);
    }
}
