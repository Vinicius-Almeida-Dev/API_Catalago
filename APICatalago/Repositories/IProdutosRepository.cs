using APICatalago.Models;

namespace APICatalago.Repositores
{
    public interface IProdutosRepository
    {
        IQueryable<Produto> GetProdutos();
        Produto GetProduto(int id);
        Produto Create(Produto produto);
        bool Update(Produto produto);
        bool Delete(int id);
    }
}
