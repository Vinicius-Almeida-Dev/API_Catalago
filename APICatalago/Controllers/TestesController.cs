using APICatalago.Context;
using APICatalago.Filters;
using APICatalago.Models;
using APICatalago.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers
{
    // atributo que habilita funcionalidades para api's
    [ApiController]

    // atributo que é utilizado para indicar a rota de acesso do controlador, no caso de agr, a rota é "produtos"
    [Route("api/[controller]")]
    public class TestesController : ControllerBase // Herdando da super classe controllerbase
    {
        private readonly AppDbContext? _context; // private = a variável criada só poderá ser acessada por está classe
                                                 // readonly = a variável criada só podera ser utilizada para leitura, nunca sobrescrita.


        private readonly IMeuServico _injetantoMeuServico;
        private readonly IConfiguration _config;
        private readonly ILogger<ApiLoggingFilter> _logger;

        public TestesController(AppDbContext context, IMeuServico injetantoMeuServico, IConfiguration config, ILogger<ApiLoggingFilter> logger)
        {
            _context = context;
            _injetantoMeuServico = injetantoMeuServico;
            _config = config;
            _logger = logger;
        }

        [HttpGet("MeuServicoTesteInjecaoDependencias")]
        public ActionResult<string> GetTestandoMeuServicoInjecaoDependecias([FromQuery] string nome)
        {
            return _injetantoMeuServico.Saudacao(nome);
        }

        //Testando from Services por meio de injeção de dependencias por inferência.
        [HttpGet("MeuServicoTesteFromServices")]
        public ActionResult<string> GetTestandoFromServices([FromServices] IMeuServico meuServico, [FromQuery] string nome)
        {
            return meuServico.Saudacao(nome);
        }

        // Testando a injeção de dependencias (DI) da interface IConfiguration - Ela serve para recuperar os valor do appsettings.json
        [HttpGet("LerArquivosConfiguracao")]
        public ActionResult<string> GetIConfiguration()
        {
            var valor1 = _config["Chave1"];
            var valor2 = _config["Chave2"];

            var Secao1 = _config["Secao1:ChaveS1"];

            return Ok($"Chave1: {valor1}\nChave2: {valor2}\nSecão 1, Chave1: {Secao1}");
        }

        // Testando extension pra middleware de tratamento de erros
        [HttpGet("TestandMiddlewereTratamentoDeErro")]
        public ActionResult GetTesteTratamentoErro()
        {
            throw new Exception("Erro");
            return Ok();
        }

        // Testando filtro de log E log criado
        [HttpGet("TestandoFilterLogging")]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult GetTesteLogging()
        {
            _logger.LogInformation("======================== TesteController >> TestandoFilterLogging ======================== ");
            return Ok();
        }



        // =========================================== COMENTARIOS CRIADOS AQUI PARA DESPOLUIR O CONTROLLER DE PRODUTOS =========================================== //

        // Criando o método GET para consultar vários produtos:
        // O AsNoTacking() é utilizado para o não rastreamento das entidades após a consulta,
        // isso só deve ser utilizado em requisições GET
        // Já o "Take(2)" é utilizado para limitar a quantidade de linhas 
        // do banco que retornará (objetos)
        // Também é valido utilização do Where

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutosAsync()
        {
            var produtos = await _context.Produtos.Take(2).AsNoTracking().ToListAsync();

            if (produtos == null)
            {
                return NotFound("Produtos não encontrador");
            }
            return produtos;
        }

        // Criando o método GET para consultar um produto:
        // Aqui a rota será chamada da seguinte forma:  URLPadrão + categorias/1/ + parêmetro "nome" com ou sem valor, pois ele já tem um valor pre definido.
        // Restrição de rota "min(1)"
        [HttpGet("TestandoFromquery")]
        //"{id:int:min(1)}/{nome=Caderno}",Name = "ObterProduto"
        public async Task<ActionResult<Produto>> GetProdutoIdAsync([FromQuery] int id, [FromQuery] string nome, [FromQuery] string textoTeste)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(produto => produto.ProdutoId == id);

            if (produto is null)
                return NotFound("Produto não encontrado");


            return produto;
        }

        // Criando o método GET para consultar o primeiro produto:
        [HttpGet("primeiro")] // Aqui a rota será chamada da seguinte forma:  URLPadrão + /Produtos/primeiro
        [HttpGet("/primeiro")] // Dois templates/endpoints de requisição para o mesmo método action, mas pode ser até mais templates.
        // Aqui irei impor uma restrição alpha para que o endpoint acesse por meio de uma instrução com limitações, pode ser utilizado em outros verbos Http.
        // Verificar mais restrições de rotas existentes futuramente - Esse recurso deve ser utilizado apenas para distinguir duas rotas parecidas, quem deve verificar e mapear as entradas de dados é o controlador
        [HttpGet("/primeiroAlpha/{valor:alpha:maxlength(5)}")]
        public async Task<ActionResult<Produto>> GetPrimeiroProdutoAsync()
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync();

            if (produto is null)
                return NotFound("Produto não encontrado");


            return produto;
        }

        // Criando o método GET para consultar o primeiro produto,  e ignorando o route controller, utilizando apenas o caminho do verbo http:
        [HttpGet("/primeiroProdutoIgnorandoRouteController")] // Aqui a rota será chamada da seguinte forma:  URLPadrão + primeiroProdutoIgnorandoRouteController
        public async Task<ActionResult<Produto>> GetPrimeiroProdutoIgnorandoRouteControllerAsync()
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync();

            if (produto is null)
                return NotFound("Produto não encontrado");


            return produto;
        }



        // Criando o método POST para criar um produto:
        [HttpPost]
        public async Task<ActionResult> PostProdutoAsync(Produto produto)
        {
            if (produto is null)
                return BadRequest("Estrutura de parâmetros enviado errado");

            await _context.Produtos.AddAsync(produto); // Criar o insert na tabela
            await _context.SaveChangesAsync(); // persiste o dado na tabela

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}/{quantidadeEstoque:int}/{param3}")]
        public async Task<ActionResult> PutProdutoAsync(int id, int quantidadeEstoque, string param3, Produto produto)
        {
            if (id != produto.ProdutoId)
                return BadRequest();

            _context.Entry(produto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProdutoAsync(int id)
        {
            // var produto = _context.Produtos.Find(id); -- Aqui o Find pega a primeira coluna da tabela, esperando que, no caso atual seja o ID, caso contrario n funcionará.
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);


            if (produto is null)
                return BadRequest();

            // _context.Entry(produto).State = EntityState.Deleted;
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return Ok(produto);

        }

        // Exemplo utilizando Trycat

        [HttpGet("{id:int}", Name = "ObterCategoriaTeste")]
        public async Task<ActionResult<Categoria>> GetCategoriaIdAsync(int id)
        {
            _logger.LogInformation("=================== VERBO: GET - /Categorias/Id ===================");
            try
            {
                var categoria = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(cat => cat.CategoriaId == id);

                if (categoria == null)
                    throw new Exception();

                return Ok(categoria);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status404NotFound, $"Categoria com o id {id} não encontrada...");
            }
        }

        // É de categoria - retirado para fazer o padrão repository
        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutosAsync()
        {
            _logger.LogInformation("=================== VERBO: GET - /Categorias/Produtos ===================");
            return await _context.Categorias.AsNoTracking().Include(p => p.Produtos).ToListAsync();
        }

    }
}
