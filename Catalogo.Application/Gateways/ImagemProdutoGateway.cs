using Catalogo.Domain.Entities;
using Catalogo.Domain.Interfaces;
using Catalogo.Infrastructure.Repositories;

namespace Catalogo.Application.Gateways
{
    public class ImagemProdutoGateway : IImagemProdutoGateway
    {
        private readonly ImagemProdutoRepository _repository;

        public ImagemProdutoGateway(ImagemProdutoRepository repository)
        {
            _repository = repository;
        }

        public async Task<ImagemProdutoEntity?> CriarImagemAsync(ImagemProdutoEntity imagem)
        {
            return await _repository.CriarImagem(imagem);
        }

        public async Task<ImagemProdutoEntity?> ObterImagemPorProdutoIdAsync(int produtoId)
        {
            return await _repository.ObterImagemPorProdutoId(produtoId);
        }

        public async Task<ImagemProdutoEntity?> AtualizarImagemAsync(ImagemProdutoEntity imagem)
        {
            return await _repository.AtualizarImagem(imagem);
        }

        public async Task<bool> RemoverImagemAsync(int produtoId)
        {
            return await _repository.RemoverImagem(produtoId);
        }
    }
}

