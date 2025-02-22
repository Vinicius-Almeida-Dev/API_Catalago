using System.ComponentModel.DataAnnotations;

namespace APICatalago.DTOs.CategoriasDTOs
{
    public class CategoriaCreateDTO
    {
        [Required]
        [StringLength(80)]
        public string? Nome { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }
    }
}
