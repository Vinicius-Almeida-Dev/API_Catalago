﻿using APICatalago.Models;
using AutoMapper;

namespace APICatalago.DTOs.Mappings
{
    public class ProdutoDTOMappingProfile : Profile
    {
        public ProdutoDTOMappingProfile()
        {
            CreateMap<Produto, ProdutoDTO>().ReverseMap();
            CreateMap<Produto,ProdutoDTOUpdateRequest>().ReverseMap();
            CreateMap<Produto, ProdutoDTOUpdateResponse>().ReverseMap();    
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            
        }
    }
}
