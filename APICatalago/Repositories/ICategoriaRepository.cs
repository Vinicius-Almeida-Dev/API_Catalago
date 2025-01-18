using APICatalago.Models;

namespace APICatalago.Repositores
{
    public interface ICategoriaRepository
    {
        IEnumerable<Categoria> GetCategorias();
        Categoria GetCategoria(int id); 
        Categoria Create(Categoria categoria);  
        Categoria Update(Categoria categoria);
        Categoria Delete(Categoria id);   
    }
}
