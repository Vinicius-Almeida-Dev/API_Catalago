using APICatalago.DTOs.CategoriasDTOs;
using APICatalago.DTOs.Mappings;
using APICatalago.DTOs.ProdutosDTOs;
using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace APICatalago.Controllers
{
    [ApiController]
    [Route("categorias")]

    public class CategoriasController : ControllerBase
    {
        private readonly ILogger<CategoriasController> _logger;
        private readonly IUnitOfWork _uof;

        public CategoriasController(IUnitOfWork uof, ILogger<CategoriasController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        [HttpGet("filtro/nome/paginacao")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPrecoPaginacaoAsync([FromQuery, Required] ParametersCategoriasFiltroNome parameters)
        {
            _logger.LogInformation("=================== VERBO: GET - /PRODUTOSPORCATEGORIA ===================");
            try
            {
                var categorias = await _uof.CategoriaHibridoRepository.GetCategoriasFiltroNomeAsync(parameters);

                if (categorias is null || !categorias.Any())
                    return NotFound("Categoria(s) não encontrada(s)");

                return ObterCategorias(categorias);

            }
            catch (Exception)
            {
                _logger.LogInformation($"Produtos não encontrados");
                return NotFound($"Produtos não encontrados");
            }
        }
        [HttpGet("com/produto")]
        public async Task<ActionResult<IEnumerable<CategoriaComProdutoDTO>>> GetCatComProdAsync()
        {
            _logger.LogInformation("=================== VERBO: GET - /Categorias Com Produtos ===================");
            try
            {
                var categorias = await _uof.CategoriaHibridoRepository.GetCategoriasComProdutosAsync();

                //var categoriasDto = CategoriaDTOMappingExtensions.ToCategoriaDTOList(categorias);

                return Ok(categorias.ToCategoriaComProdutoDTOList());

            }
            catch (Exception)
            {
                _logger.LogInformation($"Categorias não encontradas");
                return NotFound($"Categorias não encontradas");
            }

        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasAsync()
        {
            _logger.LogInformation("=================== VERBO: GET - /Categorias ===================");
            try
            {
                var categorias = await _uof.CategoriaHibridoRepository.GetAllAsync();


                return Ok(categorias.ToCategoriaDTOList());
            }
            catch (Exception)
            {
                _logger.LogInformation($"Categorias não encontradas");
                return NotFound($"Categorias não encontradas");
            }

        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> GetCategoriaIdAsync(int id)
        {
            _logger.LogInformation("=================== VERBO: GET - /Categorias/Id ===================");
            var categoria = await  _uof.CategoriaHibridoRepository.GetAsync(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogInformation($"Categoria com id {id} nao encontrada");
                return NotFound($"Categoria com id {id} nao encontrada");
            }

            return Ok(categoria.ToCategoriaDTO());
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> PostCategoriaAsync(CategoriaCreateDTO? categoriaDTO)
        {
            _logger.LogInformation("=================== VERBO: POST - /Categorias ===================");


            if (categoriaDTO is null)
            {
                _logger.LogInformation($"Dados invalidos");
                return BadRequest($"Dados invalidos");
            }
            


            var categoriaCreate = _uof.CategoriaHibridoRepository.Create(categoriaDTO.ToCategoriaCreate());
            await _uof.CommitAsync();

            
           var categoriaConvertDto =  categoriaCreate.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaConvertDto.CategoriaId }, categoriaConvertDto);

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> PutCategoriaAsync(int id, CategoriaDTO categoria)
        {
            _logger.LogInformation("=================== VERBO: PUT - /Categorias ===================");

            if (categoria.CategoriaId != id)
            {
                _logger.LogInformation($"Dados invalidos");
                return BadRequest($"Dados invalidos");
            }

            var categariaUpdate = _uof.CategoriaHibridoRepository.Update(categoria.ToCategoria());
            await _uof.CommitAsync();

            return Ok(categariaUpdate.ToCategoriaDTO());
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> DeleteProdutoAsync(int id)
        {
            _logger.LogInformation("=================== VERBO: DELETE - /Categorias ===================");
            var categoria = await _uof.CategoriaHibridoRepository.GetAsync(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogInformation($"Categoria com id {id} nao encontrada");
                return NotFound($"Categoria com id {id} nao encontrada");
            }

            var categoriaDeleted = _uof.CategoriaHibridoRepository.Delete(categoria);
            await _uof.CommitAsync();

            return Ok(categoriaDeleted.ToCategoriaDTO());

        }

        // Metodos auxiliares:
        private ActionResult<IEnumerable<ProdutoDTO>> ObterCategorias(IPagedList<Categoria> categorias)
        {
            var metaData = new
            {
                categorias.Count,
                categorias.PageSize,
                categorias.PageCount,
                categorias.TotalItemCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage
            };

            Response.Headers["X-Pagination"] = JsonConvert.SerializeObject(metaData);

            return Ok(categorias.ToCategoriaComProdutoDTOList());
        }

    }
}
