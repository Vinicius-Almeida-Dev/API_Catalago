﻿using APICatalago.DTOs.ProdutosDTOs;
using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace APICatalago.Controllers
{

    [ApiController]
    [Route("api/produtos")]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger<ProdutosController> _logger;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork uof, ILogger<ProdutosController> logger, IMapper mapper)
        {
            _uof = uof;
            _logger = logger;
            _mapper = mapper;
        }        

        [HttpGet("filtro/preco/paginacao")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPrecoPaginacaoAsync([FromQuery, Required] ParametersProdutosFiltoPreco parameters)
        {
            _logger.LogInformation("=================== VERBO: GET - /PRODUTOSPORCATEGORIA ===================");
            try
            {
                var produtos = await _uof.ProdutoHibridoRepository.GetProdutosFiltroPrecoAsync(parameters);

                if (produtos is null || !produtos.Any())
                    return NotFound("Produto(s) não encontrado(s)");

                return ObterProdutos(produtos);

            }
            catch (Exception)
            {
                _logger.LogInformation($"Produtos não encontrados");
                return NotFound($"Produtos não encontrados");
            }
        }       

        [HttpGet("paginacao")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>?>> GetProdutosPaginacao([FromQuery, Required] Parameters parameters)
        {
            _logger.LogInformation("=================== VERBO: GET - /PRODUTOSPORCATEGORIA ===================");
            try
            {
                var produtos = await _uof.ProdutoHibridoRepository.GetProdutosAsync(parameters);

                if (produtos is null || !produtos.Any())
                    return NotFound("Produto(s) não encontrado(s)");

                return ObterProdutos(produtos);

            }
            catch (Exception)
            {
                _logger.LogInformation($"Produtos não encontrados");
                return NotFound($"Produtos não encontrados");
            }
        }

        [HttpGet("por/categoria")]
        public async Task<ActionResult<ProdutoDTO>> GetProdPorCatAsync([FromQuery, Required] int id, [FromQuery] Parameters parameters)
        {
            _logger.LogInformation("=================== VERBO: GET - /PRODUTOSPORCATEGORIA ===================");
            try
            {
                var Produtos = await _uof.ProdutoHibridoRepository.GetProdutosPorCategoriaAsync(id, parameters);
            
                if (Produtos is null || !Produtos.Any())
                    return NotFound();  
                
                // var "Aqui ele virou o que deve ser" = _mapper.Map<Oque deve ser>(o que ainda é);
                // var Destino = _mapper.Map<Destino>(Origem);
                return Ok(_mapper.Map<IEnumerable<ProdutoDTO>>(Produtos));

            }
            catch (Exception)
            {
                _logger.LogInformation($"Produtos não encontrados");
                return NotFound($"Produtos não encontrados");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosAsync()
        {
            _logger.LogInformation("=================== VERBO: GET - /PRODUTOS ===================");
            try
            {
                var produtos = await _uof.ProdutoHibridoRepository.GetAllAsync();

                if (produtos is null || !produtos.Any())
                    return NotFound();

                return Ok(_mapper.Map<IEnumerable<ProdutoDTO>>(produtos));
            }
            catch (Exception)
            {
                _logger.LogInformation($"Produtos não encontrados");
                return NotFound($"Produtos não encontrados");
            }


        }

        [HttpGet("id", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> GetProdutoIdAsync([FromQuery, Required] int id)
        {
            _logger.LogInformation("=================== VERBO: GET - /PRODUTOS/TestandoFromquery ===================");
            try
            {
                var produto = await _uof.ProdutoHibridoRepository.GetAsync(p => p.ProdutoId == id);

                if(produto is null)
                    return NotFound();

                return Ok(_mapper.Map<ProdutoDTO>(produto));
            }
            catch (Exception)
            {
                _logger.LogInformation($"Produto com id {id} não encontrado");
                return NotFound($"Produto com id {id} não encontrado");
            }

        }

        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> PostProdutoAsync(ProdutoCreateDTO produtoDto)
        {
            _logger.LogInformation("=================== VERBO: POST - /PRODUTOS ===================");
            try
            {
                var produto = _mapper.Map<Produto>(produtoDto);

                var produtoCreate = _uof.ProdutoHibridoRepository.Create(produto);
                
                if(produtoCreate is null)
                    return NotFound();

               await _uof.CommitAsync();
                return new CreatedAtRouteResult("ObterProduto", new { id = produtoCreate.ProdutoId }, _mapper.Map<ProdutoDTO>(produtoCreate));
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Dados invalidos");
                return BadRequest($"Dados invalidos \nEx: {e}");
            }

        }

        [HttpPatch("{id}/UpdatePartial")]
        public async Task<ActionResult<ProdutoDTOUpdateResponse>> PatchAsync(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
        {
            if (patchProdutoDTO is null || id <= 0)
                return BadRequest();

            var produto = await _uof.ProdutoHibridoRepository.GetAsync(p => p.ProdutoId == id);
            
            if (produto is null) 
                return NotFound();

            var produtoUpRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

            patchProdutoDTO.ApplyTo(produtoUpRequest, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(produtoUpRequest))
                return BadRequest(ModelState);

           // _mapper.Map(produtoUpRequest, produto);
            _mapper.Map<ProdutoDTOUpdateRequest>(produto);


            _uof.ProdutoHibridoRepository.Update(produto);
            await _uof.CommitAsync();

            return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> PutProdutoAsync(int id, ProdutoDTO produtoDto)
        {
            _logger.LogInformation("=================== VERBO: PUT - /PRODUTOS ===================");
            try
            {
                var produto = _mapper.Map<Produto>(produtoDto);
                var produtoUpdate = _uof.ProdutoHibridoRepository.Update(produto);

                if (produtoUpdate is null)
                    return NotFound();

                await _uof.CommitAsync();

                return Ok(_mapper.Map<ProdutoDTO>(produtoUpdate));
            }
            catch (Exception)
            {
                _logger.LogInformation($"Dados invalidos");
                return BadRequest($"Dados invalidos");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProdutoAsync(int id)
        {
            _logger.LogInformation("=================== VERBO: DELETE - /PRODUTOS ===================");
            try
            {
                var produto = await _uof.ProdutoHibridoRepository.GetAsync(p => p.ProdutoId == id);


                var produtoDeleteado = _uof.ProdutoHibridoRepository.Delete(produto);

                if (produtoDeleteado is null)
                    return NotFound();

               await _uof.CommitAsync();

                return Ok(_mapper.Map<ProdutoDTO>(produtoDeleteado));
            }
            catch (Exception)
            {
                _logger.LogInformation($"Produto com id {id} não encontrado");
                return NotFound($"Produto com id {id} não encontrado");
            }

        }

        // Métodos auxiliares
        private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(IPagedList<Produto> produtos)
        {
            var metaData = new
            {
                produtos.Count,
                produtos.PageSize,
                produtos.PageCount,
                produtos.TotalItemCount,
                produtos.HasNextPage,
                produtos.HasPreviousPage
            };

            Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(metaData);

            return Ok(_mapper.Map<IEnumerable<ProdutoDTO>>(produtos));
        }
    }
}
