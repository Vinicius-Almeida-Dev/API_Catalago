using APICatalago.Context;
using APICatalago.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers
{
   
    [ApiController]    
    [Route("api/[controller]")] 
    public class ProdutosController : ControllerBase 
    {
        private readonly AppDbContext? _context; 
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(AppDbContext context, ILogger<ProdutosController> logger) 
        {
            _context = context;
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutosAsync()
        {
            _logger.LogInformation("=================== VERBO: GET - /PRODUTOS ===================");
            var produtos = await _context.Produtos.Take(2).AsNoTracking().ToListAsync();  

            if (produtos == null)
            {
                return NotFound("Produtos não encontrador");
            }
            return produtos;
        }

        [HttpGet("TestandoFromquery")]
        public async Task<ActionResult<Produto>> GetProdutoIdAsync([FromQuery]int id, [FromQuery] string nome, [FromQuery] string textoTeste)
        {
            _logger.LogInformation("=================== VERBO: GET - /PRODUTOS/TestandoFromquery ===================");
            var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(produto => produto.ProdutoId == id);

            if (produto is null)            
                return NotFound("Produto não encontrado");
            

            return produto;
        }
       
        [HttpGet("primeiro")]
        [HttpGet("/primeiroAlpha/{valor:alpha:maxlength(5)}")]
        public async Task<ActionResult<Produto>> GetPrimeiroProdutoAsync()
        {
            _logger.LogInformation("=================== VERBO: GET - //primeiroAlpha/ ===================");
            var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync();

            if (produto is null)
                return NotFound("Produto não encontrado");


            return produto;
        }
       
        [HttpGet("/primeiroProdutoIgnorandoRouteController")] 
        public async Task<ActionResult<Produto>> GetPrimeiroProdutoIgnorandoRouteControllerAsync()
        {
            _logger.LogInformation("=================== VERBO: GET - /primeiroProdutoIgnorandoRouteController ===================");
            var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync();

            if (produto is null)
                return NotFound("Produto não encontrado");


            return produto;
        }

        [HttpPost]
        public async Task<ActionResult> PostProdutoAsync(Produto produto)
        {
            _logger.LogInformation("=================== VERBO: POST - /PRODUTOS ===================");
            if (produto is null)
                return BadRequest("Estrutura de parâmetros enviado errado");

            await _context.Produtos.AddAsync(produto); 
            await _context.SaveChangesAsync();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}/{quantidadeEstoque:int}/{param3}")]
        public async Task<ActionResult> PutProdutoAsync(int id, int quantidadeEstoque, string param3, Produto produto) 
        {
            _logger.LogInformation("=================== VERBO: PUT - /PRODUTOS ===================");
            if (id != produto.ProdutoId)
                return BadRequest();

            _context.Entry(produto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return  Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProdutoAsync(int id)
        {
            _logger.LogInformation("=================== VERBO: DELETE - /PRODUTOS ===================");
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);           

            if ( produto is null)
                return BadRequest();

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return Ok(produto);
        }
    }
}
