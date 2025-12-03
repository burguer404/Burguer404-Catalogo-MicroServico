using Catalogo.Domain.Arguments.Base;

namespace Catalogo.Domain.Arguments
{
    public class ProdutoResponse : ArgumentBase
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaDescricao { get; set; } = string.Empty;
        public string ImagemBase64 { get; set; } = string.Empty;
        public byte[]? ImagemByte { get; set; }
        public bool Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
