using Catalogo.Domain.Arguments.Base;

namespace Catalogo.Domain.Arguments
{
    public class CardapioResponse : ResponseBase<object>
    {
        public List<CategoriaResponse> Categorias { get; set; } = new List<CategoriaResponse>();
        public int TotalProdutos { get; set; }
        public DateTime DataAtualizacao { get; set; } = DateTime.Now;
    }
}