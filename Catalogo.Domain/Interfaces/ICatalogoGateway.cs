using Catalogo.Domain.Entities;
using Catalogo.Domain.Arguments;

namespace Catalogo.Domain.Interfaces
{
    public interface ICatalogoGateway
    {
        Task<ProdutoEntity?> CriarProdutoAsync(ProdutoEntity produto);
        Task<List<ProdutoEntity>?> ListarProdutosAsync();
        Task<ProdutoEntity?> ObterProdutoPorIdAsync(int id);
        Task<ProdutoEntity?> AtualizarProdutoAsync(ProdutoEntity produto);
        Task<bool> RemoverProdutoAsync(int id);
        Task<List<ProdutoEntity>?> ObterProdutosPorCategoriaAsync(int categoriaId);
        Task<CardapioResponse> ObterCardapioAsync();
        Task<List<CategoriaEntity>?> ListarCategoriasAsync();
        Task<CategoriaEntity?> CriarCategoriaAsync(CategoriaEntity categoria);
    }
}
