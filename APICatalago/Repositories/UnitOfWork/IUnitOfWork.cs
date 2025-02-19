using APICatalago.Repositories.hybrid.Interfaces;

namespace APICatalago.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICategoriaHibridoRepository? CategoriaHibridoRepository { get; }
        IProdutoHibridoRepository? ProdutoHibridoRepository { get; }
        Task CommitAsync();
    }
}
