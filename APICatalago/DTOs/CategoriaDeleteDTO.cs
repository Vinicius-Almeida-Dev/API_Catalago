using System.ComponentModel.DataAnnotations;

namespace APICatalago.DTOs
{
    public class CategoriaDeleteDTO
    {
        [Required]
        [StringLength(80)]
        public string? Nome { get; set; }
    }
}
