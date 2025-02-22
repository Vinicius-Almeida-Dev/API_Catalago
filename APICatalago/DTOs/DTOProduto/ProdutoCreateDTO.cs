using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APICatalago.DTOs.ProdutosDTOs
{
    public class ProdutoCreateDTO
    {
        [Required]
        [StringLength(80)]
        public string? Nome { get; set; }

        [Required]
        [StringLength(300)]
        public string? Descricao { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }

        public int CategoriaId { get; set; }
    }
}
