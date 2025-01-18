using APICatalago.Context;
using APICatalago.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Repositores
{
    public class ProdutosRepository : IProdutosRepository
    {
        private readonly AppDbContext _context;

        public ProdutosRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Produto> GetProdutos()
        {
            var produtos = _context.Produtos;
            
            if (produtos is null)
                throw new ArgumentNullException(nameof(produtos));
            
            return produtos; 
        }

        public Produto GetProduto(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if (produto is null)
                throw new ArgumentNullException(nameof(produto));

            return produto;
        }

        public Produto Create(Produto produto)
        {
            if (produto is null)
                throw new ArgumentNullException(nameof(produto));

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return produto;
        }
        
        public bool Update(Produto produto)
        {
            if (produto is null)
                throw new ArgumentNullException(nameof(produto));

            if (_context.Produtos.Any(p => produto.ProdutoId == produto.ProdutoId))
            {
                _context.Produtos.Update(produto);
                _context.SaveChanges();
                return true;
            }

            return false;
            
        }

        // Esse método tbm poderia ser boleano
        public bool Delete(int id)
        {
            var produto = GetProduto(id);

            if (produto is not null)
            {
                _context.Produtos.Remove(produto);
                _context.SaveChanges();
                return true;
            }

            return false;           
        }
    }
}
