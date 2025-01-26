using APICatalago.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace APICatalago.DTOs
{
    public class CategoriaComProdutoDTO
    {
        [Key]
        public int CategoriaId { get; set; }

        [Required]
        [StringLength(80)]
        public string? Nome { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }

        [Required]
        public ICollection<Produto>? Produtos { get; set; }

        public CategoriaComProdutoDTO()
        {
            Produtos = new Collection<Produto>();
        }
    }
}
