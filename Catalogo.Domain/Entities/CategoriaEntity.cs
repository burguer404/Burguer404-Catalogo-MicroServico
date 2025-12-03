namespace Catalogo.Domain.Entities
{
    public class CategoriaEntity
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public bool Ativa { get; set; } = true;
    }
}
