﻿using APICatalago.DTOs;
using APICatalago.Models;

namespace APICatalago.Mappings
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
        public static Categoria? ToCategoria(CategoriaDTO categoriaDTO)
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
    }
}
