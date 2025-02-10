using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories.Generic.Interface;
using System.Linq.Expressions;

namespace APICatalago.Repositories.hybrid.Interfaces
{
    public interface IProdutoHibridoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosPorCategoria(int id, Parameters parameters);
        PagedList<Produto> GetProdutos(Parameters parameters);
        PagedList<Produto> GetProdutosFiltroPreco(ParametersProdutosFiltoPreco produtosFiltoPreco);
    }
}
