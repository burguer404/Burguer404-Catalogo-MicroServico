using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalogo.Domain.Entities
{
    public class ImagemProdutoEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("produtoId")]
        public int ProdutoId { get; set; }

        [BsonElement("imagemByte")]
        public byte[] ImagemByte { get; set; } = Array.Empty<byte>();

    }
}

