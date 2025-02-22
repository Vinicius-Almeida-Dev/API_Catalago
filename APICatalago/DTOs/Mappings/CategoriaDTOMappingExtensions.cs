using APICatalago.DTOs.CategoriasDTOs;
using APICatalago.Models;

namespace APICatalago.DTOs.Mappings
{
    public static class CategoriaDTOMappingExtensions
    {
        public static CategoriaDTO? ToCategoriaDTO(this Categoria categoria)
        {
            if (categoria is null)
                return null;

            return new CategoriaDTO
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl
            };
        }

        public static Categoria? ToCategoria(this CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO is null)
                return null;

            return new Categoria()
            {
                CategoriaId = categoriaDTO.CategoriaId,
                Nome = categoriaDTO.Nome,
                ImagemUrl = categoriaDTO.ImagemUrl
            };
        }

        public static Categoria? ToCategoriaCreate(this CategoriaCreateDTO categoriaDTO)
        {
            if (categoriaDTO is null)
                return null;

            return new Categoria()
            {
                Nome = categoriaDTO.Nome,
                ImagemUrl = categoriaDTO.ImagemUrl
            };
        }

        public static IEnumerable<CategoriaDTO>? ToCategoriaDTOList(this IEnumerable<Categoria> categorias)
        {
            if (!categorias.Any())
                return new List<CategoriaDTO>();

            return categorias.Select(categoria => new CategoriaDTO
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl
            }).ToList();
        }

        public static IEnumerable<CategoriaComProdutoDTO>? ToCategoriaComProdutoDTOList(this IEnumerable<Categoria> categorias)
        {
            if (!categorias.Any())
                return new List<CategoriaComProdutoDTO>();

            return categorias.Select(categoria => new CategoriaComProdutoDTO
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl,
                Produtos = categoria.Produtos

            }).ToList();
        }

          public static CategoriaDeleteDTO ToCategoriaDeleteDTO(this Categoria categoria)
        {
            if(categoria is null)
                return new CategoriaDeleteDTO();

            return new CategoriaDeleteDTO
            {
                Nome = categoria.Nome
            };
        }
    }
}
