using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories.Generic.Interface;

namespace APICatalago.Repositories.hybrid.Interfaces
{
    public interface ICategoriaHibridoRepository :IRepository<Categoria>
    {
        IEnumerable<Categoria> GetCategoriasComProdutos();
        PagedList<Categoria> GetCategoriasFiltroNome(ParametersCategoriasFiltroNome categoriasParams);
    }
}

