using APICatalago.DTOs;
using APICatalago.Models;
using APICatalago.Repositories.UnitOfWork;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork uof, ILogger<ProdutosController> logger, IMapper mapper)
        {
            _uof = uof;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("ProdutosPorCategoria ")]
        public ActionResult<ProdutoDTO> GetProdPorCat([FromQuery, Required] int id)
        {
            _logger.LogInformation("=================== VERBO: GET - /PRODUTOSPORCATEGORIA ===================");
            try
            {
                var Produtos = _uof.ProdutoHibridoRepository.GetProdutosPorCategoria(id);
            
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
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosAsync()
        {
            _logger.LogInformation("=================== VERBO: GET - /PRODUTOS ===================");
            try
            {
                var produtos = _uof.ProdutoHibridoRepository.GetAll().OrderByDescending(p => p.ProdutoId);

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

        [HttpGet("produto", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> GetProdutoIdAsync([FromQuery, Required] int id)
        {
            _logger.LogInformation("=================== VERBO: GET - /PRODUTOS/TestandoFromquery ===================");
            try
            {
                var produto = _uof.ProdutoHibridoRepository.Get(p => p.ProdutoId == id);

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
        public ActionResult<ProdutoDTO> PostProdutoAsync(ProdutoDTO produtoDto)
        {
            _logger.LogInformation("=================== VERBO: POST - /PRODUTOS ===================");
            try
            {
                var produto = _mapper.Map<Produto>(produtoDto);

                var produtoCreate = _uof.ProdutoHibridoRepository.Create(produto);
                
                if(produtoCreate is null)
                    return NotFound();

                _uof.Commit();
                return new CreatedAtRouteResult("ObterProduto", new { id = produtoCreate.ProdutoId }, _mapper.Map<ProdutoDTO>(produtoCreate));
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Dados invalidos");
                return BadRequest($"Dados invalidos \nEx: {e}");
            }

        }

        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> PutProdutoAsync(int id, ProdutoDTO produtoDto)
        {
            _logger.LogInformation("=================== VERBO: PUT - /PRODUTOS ===================");
            try
            {
                var produto = _mapper.Map<Produto>(produtoDto);
                var produtoUpdate = _uof.ProdutoHibridoRepository.Update(produto);

                if (produtoUpdate is null)
                    return NotFound();

                _uof.Commit();

                return Ok(_mapper.Map<ProdutoDTO>(produtoUpdate));
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

                if (produtoDeleteado is null)
                    return NotFound();

                _uof.Commit();

                return Ok(_mapper.Map<ProdutoDTO>(produtoDeleteado));
            }
            catch (Exception)
            {
                _logger.LogInformation($"Produto com id {id} não encontrado");
                return NotFound($"Produto com id {id} não encontrado");
            }

        }
    }
}
