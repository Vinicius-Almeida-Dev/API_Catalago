using APICatalago.DTOs;
using APICatalago.Mappings;
using APICatalago.Models;
using APICatalago.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace APICatalago.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class CategoriasController : ControllerBase
    {
        private readonly ILogger<CategoriasController> _logger;
        private readonly IUnitOfWork _uof;

        public CategoriasController(IUnitOfWork uof, ILogger<CategoriasController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        [HttpGet("CategoriaComProduto")]
        public ActionResult<IEnumerable<CategoriaComProdutoDTO>> GetCatComProd()
        {
            _logger.LogInformation("=================== VERBO: GET - /Categorias Com Produtos ===================");
            try
            {
                var categorias = _uof.CategoriaHibridoRepository.GetCategoriasComProdutos();

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
        public ActionResult<IEnumerable<Categoria>> GetCategoriasAsync()
        {
            _logger.LogInformation("=================== VERBO: GET - /Categorias ===================");
            try
            {                
                var categorias = _uof.CategoriaHibridoRepository.GetAll();


                return Ok(categorias.ToCategoriaDTOList());
            }
            catch (Exception)
            {
                _logger.LogInformation($"Categorias não encontradas");
                return NotFound($"Categorias não encontradas");
            }
            
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> GetCategoriaIdAsync(int id)
        {
            _logger.LogInformation("=================== VERBO: GET - /Categorias/Id ===================");
            var categoria = _uof.CategoriaHibridoRepository.Get(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogInformation($"Categoria com id {id} nao encontrada");
                return NotFound($"Categoria com id {id} nao encontrada");
            }

            return Ok(categoria.ToCategoriaDTO());
        }

        [HttpPost]
        public ActionResult PostCategoriaAsync(Categoria categoria)
        {
            _logger.LogInformation("=================== VERBO: POST - /Categorias ===================");


            if (categoria is null)
            {
                _logger.LogInformation($"Dados invalidos");
                return BadRequest($"Dados invalidos");
            }

            var categoriaCreate = _uof.CategoriaHibridoRepository.Create(categoria);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCreate.CategoriaId }, categoria);

        }

        [HttpPut("{id:int}")]
        public ActionResult PutCategoriaAsync(int id, Categoria categoria)
        {
            _logger.LogInformation("=================== VERBO: PUT - /Categorias ===================");

            if (categoria.CategoriaId != id)
            {
                _logger.LogInformation($"Dados invalidos");
                return BadRequest($"Dados invalidos");
            }

            var categariaUpdate = _uof.CategoriaHibridoRepository.Update(categoria);
            _uof.Commit();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteProdutoAsync(int id)
        {
            _logger.LogInformation("=================== VERBO: DELETE - /Categorias ===================");
            var categoria = _uof.CategoriaHibridoRepository.Get(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogInformation($"Categoria com id {id} nao encontrada");
                return NotFound($"Categoria com id {id} nao encontrada");
            }

            var categoriaDeleted = _uof.CategoriaHibridoRepository.Delete(categoria);
            _uof.Commit();

            return Ok(categoriaDeleted);

        }

    }
}
