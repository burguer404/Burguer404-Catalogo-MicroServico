using Catalogo.Application.Presenters;
using Catalogo.Application.UseCases;
using Catalogo.Domain.Arguments;
using Catalogo.Domain.Arguments.Base;
using Catalogo.Domain.Interfaces;

namespace Catalogo.Application.Controllers
{
    public class ProdutoController
    {
        private ICatalogoGateway _gateway;
        private IImagemProdutoGateway _imagemGateway;

        public ProdutoController(ICatalogoGateway gateway, IImagemProdutoGateway imagemGateway)
        {
            _gateway = gateway;
            _imagemGateway = imagemGateway;
        }

        public async Task<ResponseBase<ProdutoResponse>> CadastrarProduto(ProdutoRequest request)
        {
            var useCase = CriarProdutoUseCase.Create(_gateway, _imagemGateway);
            var imagemEmBytes = await useCase.ConverterMemoryStream(request.Imagem);
            var response = await useCase.ExecuteAsync(request, imagemEmBytes ?? []);
            return response;
        }

        public async Task<ResponseBase<ProdutoResponse>> ListarProdutos()
        {
            var useCase = ListarProdutosUseCase.Create(_gateway, _imagemGateway);
            var response = await useCase.ExecuteAsync();
            return response;
        }

        public async Task<ResponseBase<ProdutoResponse>> AtualizarProduto(ProdutoRequest request)
        {
            var useCase = AtualizarProdutoUseCase.Create(_gateway, _imagemGateway);
            var response = await useCase.ExecuteAsync(request);
            return response;
        }

        public async Task<ResponseBase<bool>> RemoverProduto(int produtoId)
        {
            var useCase = RemoverProdutoUseCase.Create(_gateway, _imagemGateway);
            var response = await useCase.ExecuteAsync(produtoId);
            return response;
        }

        public async Task<ResponseBase<CardapioResponse>> ObterCardapio()
        {
            var produtos = await ListarProdutos();

            if (!produtos.Sucesso)
                return new ResponseBase<CardapioResponse>();

            var itensCardapio = new List<ProdutoResponse>(produtos.Resultado!);

            return CatalogoPresenter.ObterCardapioResponse(itensCardapio);
        }

        public async Task<ResponseBase<string>> VisualizarImagem(int produtoId)
        {
            var useCase = VisualizarImagemProdutoUseCase.Create(_imagemGateway);
            var response = await useCase.ExecuteAsync(produtoId);
            return response;
        }

        public async Task<ResponseBase<ProdutoResponse>> ObterProdutosPorCategoria(int categoriaId)
        {
            var useCase = ObterProdutosPorCategoriaUseCase.Create(_gateway, _imagemGateway);
            var response = await useCase.ExecuteAsync(categoriaId);
            return response;
        }
    }
}
