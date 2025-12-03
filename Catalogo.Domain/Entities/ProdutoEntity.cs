using Catalogo.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalogo.Domain.Entities
{
    public class ProdutoEntity : EntityBase
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int CategoriaId { get; set; }
        public virtual CategoriaEntity Categoria { get; set; } = null!;
        public bool Status { get; set; } = true;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }

        [NotMapped]
        public string ImagemBase64 { get; set; } = string.Empty;
    }
}
