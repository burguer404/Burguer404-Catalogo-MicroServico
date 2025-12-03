using Catalogo.Domain.Entities;
using MongoDB.Driver;

namespace Catalogo.Infrastructure.Repositories
{
    public class ImagemProdutoRepository
    {
        private readonly IMongoCollection<ImagemProdutoEntity> _collection;

        public ImagemProdutoRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<ImagemProdutoEntity>("ImagensProdutos");
        }

        public async Task<ImagemProdutoEntity?> CriarImagem(ImagemProdutoEntity imagem)
        {
            await _collection.InsertOneAsync(imagem);
            return imagem;
        }

        public async Task<ImagemProdutoEntity?> ObterImagemPorProdutoId(int produtoId)
        {
            var filter = Builders<ImagemProdutoEntity>.Filter.Eq(x => x.ProdutoId, produtoId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<ImagemProdutoEntity?> AtualizarImagem(ImagemProdutoEntity imagem)
        {
            var filter = Builders<ImagemProdutoEntity>.Filter.Eq(x => x.ProdutoId, imagem.ProdutoId);
            var update = Builders<ImagemProdutoEntity>.Update
                .Set(x => x.ImagemByte, imagem.ImagemByte);

            var options = new FindOneAndUpdateOptions<ImagemProdutoEntity>
            {
                ReturnDocument = ReturnDocument.After
            };

            return await _collection.FindOneAndUpdateAsync(filter, update, options);
        }

        public async Task<bool> RemoverImagem(int produtoId)
        {
            var filter = Builders<ImagemProdutoEntity>.Filter.Eq(x => x.ProdutoId, produtoId);
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}

