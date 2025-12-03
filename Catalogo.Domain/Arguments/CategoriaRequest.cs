using Catalogo.Domain.Arguments.Base;

namespace Catalogo.Domain.Arguments
{
    public class CategoriaRequest : ArgumentBase
    {
        public string Descricao { get; set; } = string.Empty;
        public bool Ativa { get; set; } = true;
    }
}
