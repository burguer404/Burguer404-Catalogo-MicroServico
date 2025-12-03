using Catalogo.Domain.Entities;

namespace Catalogo.Domain.Interfaces
{
    public interface IImagemProdutoGateway
    {
        Task<ImagemProdutoEntity?> CriarImagemAsync(ImagemProdutoEntity imagem);
        Task<ImagemProdutoEntity?> ObterImagemPorProdutoIdAsync(int produtoId);
        Task<ImagemProdutoEntity?> AtualizarImagemAsync(ImagemProdutoEntity imagem);
        Task<bool> RemoverImagemAsync(int produtoId);
    }
}

