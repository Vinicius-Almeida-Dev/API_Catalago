using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories.Generic.Interface;
using X.PagedList;

namespace APICatalago.Repositories.hybrid.Interfaces
{
    public interface ICategoriaHibridoRepository :IRepository<Categoria>
    {
        Task<IEnumerable<Categoria>> GetCategoriasComProdutosAsync();
        Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(ParametersCategoriasFiltroNome categoriasParams);
    }
}

