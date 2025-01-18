using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Repositores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class CategoriasController : ControllerBase
    {
        private readonly ILogger<CategoriasController> _logger;
        private readonly ICategoriaRepository _cRepository;

        public CategoriasController(ILogger<CategoriasController> logger, ICategoriaRepository cRepository)
        {
            _logger = logger;
            _cRepository = cRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasAsync()
        {
            _logger.LogInformation("=================== VERBO: GET - /Categorias ===================");
            var categorias = _cRepository.GetCategorias();
            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> GetCategoriaIdAsync(int id)
        {
            _logger.LogInformation("=================== VERBO: GET - /Categorias/Id ===================");
            var categoria = _cRepository.GetCategoria(id);
            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult PostCategoriaAsync(Categoria categoria)
        {
            _logger.LogInformation("=================== VERBO: POST - /Categorias ===================");
            var categoriaCreate = _cRepository.Create(categoria);
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCreate.CategoriaId }, categoriaCreate);

        }

        [HttpPut("{id:int}")]
        public ActionResult PutCategoriaAsync(int id, Categoria categoria)
        {
            _logger.LogInformation("=================== VERBO: PUT - /Categorias ===================");
            var categariaUpdate = _cRepository.Update(categoria);
            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteProdutoAsync(int id)
        {
            _logger.LogInformation("=================== VERBO: DELETE - /Categorias ===================");
            var categoriaDeleted = _cRepository.Delete(id);
            return Ok(categoriaDeleted);

        }

    }
}
