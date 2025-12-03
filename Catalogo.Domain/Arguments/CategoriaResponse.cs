using Catalogo.Domain.Arguments.Base;

namespace Catalogo.Domain.Arguments
{
    public class CategoriaResponse : ArgumentBase
    {
        public string Descricao { get; set; } = string.Empty;
        public bool Ativa { get; set; }
        public List<ProdutoResponse> Produtos { get; set; } = new List<ProdutoResponse>();
    }
}
