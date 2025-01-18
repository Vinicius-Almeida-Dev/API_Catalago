using APICatalago.Context;
using APICatalago.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Repositores
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Categoria> GetCategorias()
        {
            var categorias = _context.Categorias.ToList();

            if (categorias != null)
                throw new ArgumentNullException(nameof(categorias));

            return categorias;
        }

        public Categoria GetCategoria(int id)
        {
            return _context.Categorias.Find(id); // O find é utilizado para encontrar a chave primareia na tabela.
        }

        public Categoria Create(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return categoria;
        }

        public Categoria Update(Categoria categoria)
        {
            _context.Entry(categoria).State = EntityState.Modified; 
            _context.SaveChanges();

            return categoria;
        }

        public Categoria Delete(Categoria categoria)
        {
            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return categoria;            
        }

       
    }
}
