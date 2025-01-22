using APICatalago.Models;
using APICatalago.Repositories.Generic.Interface;
using System.Linq.Expressions;

namespace APICatalago.Repositories.hybrid.Interfaces
{
    public interface IProdutoHibridoRepository : IRepository<Produto>
    {
          IEnumerable<Produto> GetProdutosPorCategoria(int id);
    }
}
