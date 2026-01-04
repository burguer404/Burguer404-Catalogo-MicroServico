using Catalogo.Domain.Arguments.Base;
using Microsoft.AspNetCore.Http;

namespace Catalogo.Domain.Arguments
{
    public class ProdutoRequest : ArgumentBase
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public double Preco { get; set; }
        public int CategoriaId { get; set; }
        public IFormFile? Imagem { get; set; }
        public bool? Status { get; set; }
    }
}
