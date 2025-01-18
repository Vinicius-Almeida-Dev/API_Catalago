using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Repositores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.ComponentModel.DataAnnotations;

namespace APICatalago.Controllers
{
   
    [ApiController]    
    [Route("api/[controller]")] 
    public class ProdutosController : ControllerBase 
    {
        private readonly IProdutosRepository _pRepository;
        private readonly ILogger<ProdutosController> _logger;        

        public ProdutosController(IProdutosRepository pRepository, ILogger<ProdutosController> logger) 
        {
            _pRepository = pRepository;
            _logger = logger;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> GetProdutosAsync()
        {
            _logger.LogInformation("=================== VERBO: GET - /PRODUTOS ===================");
            try
            {
                var produtos = _pRepository.GetProdutos().OrderByDescending(p => p.ProdutoId);

                return Ok(produtos);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Produtos não encontrados");
                return NotFound($"Produtos não encontrados");
            }


        }

        [HttpGet("produto", Name = "ObterProduto")]
        public ActionResult<Produto> GetProdutoIdAsync([FromQuery, Required]int id)
        {
            _logger.LogInformation("=================== VERBO: GET - /PRODUTOS/TestandoFromquery ===================");
            try
            {
                var produto = _pRepository.GetProduto(id);

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
                var produtoCreate = _pRepository.Create(produto);
                return new CreatedAtRouteResult("ObterProduto", new { id = produtoCreate.ProdutoId }, produtoCreate);
            }
            catch (Exception)
            {
                _logger.LogInformation($"Dados invalidos");
                return BadRequest($"Dados invalidos");
            }
            
        }

        [HttpPut("{id:int}")]
        public ActionResult PutProdutoAsync(int id, Produto produto) 
        {
            _logger.LogInformation("=================== VERBO: PUT - /PRODUTOS ===================");
            try
            {
                var produtoUpdate = _pRepository.Update(produto);   
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
                var produtoUpdate = _pRepository.Delete(id);
                return Ok(produtoUpdate);
            }
            catch (Exception)
            {
                _logger.LogInformation($"Produto com id {id} não encontrado");
                return NotFound($"Produto com id {id} não encontrado");
            }
          
        }
    }
}
