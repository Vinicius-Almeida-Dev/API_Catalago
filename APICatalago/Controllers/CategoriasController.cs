using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Repositories.Generic.Interface;
using APICatalago.Repositories.Specific.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class CategoriasController : ControllerBase
    {
        private readonly ILogger<CategoriasController> _logger;
        private readonly IRepository<Categoria> _repository; // Olhando para o repositório genérico

        public CategoriasController(ILogger<CategoriasController> logger, IRepository<Categoria> cRepository)
        {
            _logger = logger;
            _repository = cRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasAsync()
        {
            _logger.LogInformation("=================== VERBO: GET - /Categorias ===================");
            try
            {                
                var categorias = _repository.GetAll();

                return Ok(categorias);

            }
            catch (Exception)
            {
                _logger.LogInformation($"Categorias não encontradas");
                return NotFound($"Categorias não encontradas");
            }
            
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> GetCategoriaIdAsync(int id)
        {
            _logger.LogInformation("=================== VERBO: GET - /Categorias/Id ===================");
            var categoria = _repository.Get(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogInformation($"Categoria com id {id} nao encontrada");
                return NotFound($"Categoria com id {id} nao encontrada");
            }

            return Ok(categoria);
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

            var categoriaCreate = _repository.Create(categoria);
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

            var categariaUpdate = _repository.Update(categoria);
            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteProdutoAsync(int id)
        {
            _logger.LogInformation("=================== VERBO: DELETE - /Categorias ===================");
            var categoria = _repository.Get(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogInformation($"Categoria com id {id} nao encontrada");
                return NotFound($"Categoria com id {id} nao encontrada");
            }

            var categoriaDeleted = _repository.Delete(categoria);
            return Ok(categoriaDeleted);

        }

    }
}
