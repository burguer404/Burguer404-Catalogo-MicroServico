using Catalogo.Domain.Entities;
using Catalogo.Domain.Arguments;
using Catalogo.Domain.Interfaces;
using Catalogo.Infrastructure.Repositories;

namespace Catalogo.Application.Gateways
{
    public class CatalogoGateway : ICatalogoGateway
    {
        private readonly CatalogoRepository _repository;

        public CatalogoGateway(CatalogoRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProdutoEntity?> CriarProdutoAsync(ProdutoEntity produto)
        {
            return await _repository.CriarProduto(produto);
        }

        public async Task<List<ProdutoEntity>?> ListarProdutosAsync()
        {
            return await _repository.ListarProdutos();
        }

        public async Task<ProdutoEntity?> ObterProdutoPorIdAsync(int id)
        {
            return await _repository.ObterProdutoPorId(id);
        }

        public async Task<ProdutoEntity?> AtualizarProdutoAsync(ProdutoEntity produto)
        {
            return await _repository.AtualizarProduto(produto);
        }

        public async Task<bool> RemoverProdutoAsync(int id)
        {
            return await _repository.RemoverProduto(id);
        }

        public async Task<List<ProdutoEntity>?> ObterProdutosPorCategoriaAsync(int categoriaId)
        {
            return await _repository.ObterProdutosPorCategoria(categoriaId);
        }

        public async Task<CardapioResponse> ObterCardapioAsync()
        {
            return await _repository.ObterCardapio();
        }

        public async Task<List<CategoriaEntity>?> ListarCategoriasAsync()
        {
            return await _repository.ListarCategorias();
        }

        public async Task<CategoriaEntity?> CriarCategoriaAsync(CategoriaEntity categoria)
        {
            return await _repository.CriarCategoria(categoria);
        }
    }
}
