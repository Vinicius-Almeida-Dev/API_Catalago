using APICatalago.Models;
using APICatalago.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace APICatalago.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(IUnitOfWork uof, ILogger<ProdutosController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        [HttpGet("ProdutosPorCategoria ")]
        public IActionResult GetProdPorCat([FromQuery, Required] int id)
        {
            _logger.LogInformation("=================== VERBO: GET - /PRODUTOSPORCATEGORIA ===================");
            try
            {
                var result = _uof.ProdutoHibridoRepository.GetProdutosPorCategoria(id);
                return Ok(result);


            }
            catch (Exception)
            {
                _logger.LogInformation($"Produtos não encontrados");
                return NotFound($"Produtos não encontrados");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> GetProdutosAsync()
        {
            _logger.LogInformation("=================== VERBO: GET - /PRODUTOS ===================");
            try
            {
                var produtos = _uof.ProdutoHibridoRepository.GetAll().OrderByDescending(p => p.ProdutoId);

                return Ok(produtos);
            }
            catch (Exception)
            {
                _logger.LogInformation($"Produtos não encontrados");
                return NotFound($"Produtos não encontrados");
            }


        }

        [HttpGet("produto", Name = "ObterProduto")]
        public ActionResult<Produto> GetProdutoIdAsync([FromQuery, Required] int id)
        {
            _logger.LogInformation("=================== VERBO: GET - /PRODUTOS/TestandoFromquery ===================");
            try
            {
                var produto = _uof.ProdutoHibridoRepository.Get(p => p.ProdutoId == id);

                return Ok(produto);
            }
            catch (Exception)
            {
                _logger.LogInformation($"Produto com id {id} não encontrado");
                return NotFound($"Produto com id {id} não encontrado");
            }

        }

        [HttpPost]
        public ActionResult PostProdutoAsync(Produto produto)
        {
            _logger.LogInformation("=================== VERBO: POST - /PRODUTOS ===================");
            try
            {
                var produtoCreate = _uof.ProdutoHibridoRepository.Create(produto);
                _uof.Commit();
                return new CreatedAtRouteResult("ObterProduto", new { id = produtoCreate.ProdutoId }, produtoCreate);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Dados invalidos");
                return BadRequest($"Dados invalidos \nEx: {e}");
            }

        }

        [HttpPut("{id:int}")]
        public ActionResult PutProdutoAsync(int id, Produto produto)
        {
            _logger.LogInformation("=================== VERBO: PUT - /PRODUTOS ===================");
            try
            {
                var produtoUpdate = _uof.ProdutoHibridoRepository.Update(produto);
                _uof.Commit();
                return Ok(produtoUpdate);
            }
            catch (Exception)
            {
                _logger.LogInformation($"Dados invalidos");
                return BadRequest($"Dados invalidos");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteProdutoAsync(int id)
        {
            _logger.LogInformation("=================== VERBO: DELETE - /PRODUTOS ===================");
            try
            {
                var produto = _uof.ProdutoHibridoRepository.Get(p => p.ProdutoId == id);


                var produtoDeleteado = _uof.ProdutoHibridoRepository.Delete(produto);
                _uof.Commit();
                return Ok(produtoDeleteado);
            }
            catch (Exception)
            {
                _logger.LogInformation($"Produto com id {id} não encontrado");
                return NotFound($"Produto com id {id} não encontrado");
            }

        }
    }
}
