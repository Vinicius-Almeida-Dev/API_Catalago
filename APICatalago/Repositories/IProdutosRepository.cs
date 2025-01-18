using APICatalago.Models;

namespace APICatalago.Repositores
{
    public interface IProdutosRepository
    {
        IEnumerable<Produto> GetProdutos();
        Produto GetProduto(int id);
        Produto Create(Produto produto);
        Produto Update(Produto produto);
        Produto Delete(int id);
    }
}
