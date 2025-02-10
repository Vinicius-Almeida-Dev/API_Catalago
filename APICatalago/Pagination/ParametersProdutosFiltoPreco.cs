namespace APICatalago.Pagination
{
    public class ParametersProdutosFiltoPreco : Parameters
    {
        public decimal? Preco { get; set; }
        public string? PrecoCriterio { get; set; } // "maior", "menor", "igual"
    }
}
