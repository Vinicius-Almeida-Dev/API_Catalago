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
        private readonly AppDbContext _context;
        private readonly ILogger<CategoriasController> _logger;
        private readonly CategoriaRepository _cRepository;
        private readonly ProdutosRepository _pRepository;

        public CategoriasController(AppDbContext context, ILogger<CategoriasController> logger, CategoriaRepository cRepository, ProdutosRepository pRepository)
        {
            _context = context;
            _logger = logger;
            _cRepository = cRepository;
            _pRepository = pRepository; 
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutosAsync()
        {
            _logger.LogInformation("=================== VERBO: GET - /Categorias/Produtos ===================");
            return await _context.Categorias.AsNoTracking().Include(p => p.Produtos).ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasAsync()
        {
            _logger.LogInformation("=================== VERBO: GET - /Categorias ===================");

            var categorias = await _context.Categorias.AsNoTracking().ToListAsync();

            if (categorias == null)
                return BadRequest();

            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<Categoria>> GetCategoriaIdAsync(int id)
        {
            _logger.LogInformation("=================== VERBO: GET - /Categorias/Id ===================");
          
                var categoria = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(cat => cat.CategoriaId == id);

                if (categoria == null)
                    throw new Exception();

                return Ok(categoria);            
        }

        [HttpPost]
        public async Task<ActionResult> PostCategoriaAsync(Categoria categoria)
        {
            _logger.LogInformation("=================== VERBO: POST - /Categorias ===================");

              await _context.Categorias.AddAsync(categoria); 
                await _context.SaveChangesAsync();

                return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
                    
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutCategoriaAsync(int id, Categoria categoria)
        {
            _logger.LogInformation("=================== VERBO: PUT - /Categorias ===================");
            if (id != categoria.CategoriaId)
                return BadRequest();

            _context.Entry(categoria).State = EntityState.Modified;
           await _context.SaveChangesAsync();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProdutoAsync(int id)
        {
            _logger.LogInformation("=================== VERBO: DELETE - /Categorias ===================");
            var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);

            if (categoria is null)
                return BadRequest();

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return Ok(categoria);

        }

    }
}
