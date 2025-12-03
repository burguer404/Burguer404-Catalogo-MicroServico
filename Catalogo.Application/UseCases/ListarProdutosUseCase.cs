using Catalogo.Domain.Arguments;
using Catalogo.Domain.Arguments.Base;
using Catalogo.Domain.Interfaces;
using Catalogo.Application.Presenters;

namespace Catalogo.Application.UseCases
{
    public class ListarProdutosUseCase
    {
        private readonly ICatalogoGateway _gateway;
        private readonly IImagemProdutoGateway _imagemGateway;

        public ListarProdutosUseCase(ICatalogoGateway gateway, IImagemProdutoGateway imagemGateway)
        {
            _gateway = gateway;
            _imagemGateway = imagemGateway;
        }

        public static ListarProdutosUseCase Create(ICatalogoGateway gateway, IImagemProdutoGateway imagemGateway)
        {
            return new ListarProdutosUseCase(gateway, imagemGateway);
        }

        public async Task<ResponseBase<ProdutoResponse>> ExecuteAsync()
        {
            var produtos = await _gateway.ListarProdutosAsync();
            
            if (produtos == null)
            {
                return new ResponseBase<ProdutoResponse>() { Sucesso = false, Mensagem = "Erro ao listar produtos", Resultado = [] };
            }

            var imagensPorProdutoId = new Dictionary<int, string>();
            foreach (var produto in produtos)
            {
                var imagem = await _imagemGateway.ObterImagemPorProdutoIdAsync(produto.Id);
                if (imagem != null && imagem.ImagemByte != null && imagem.ImagemByte.Length > 0)
                {
                    imagensPorProdutoId[produto.Id] = "data:image/png;base64," + Convert.ToBase64String(imagem.ImagemByte);
                }
            }

            return CatalogoPresenter.ObterListaProdutoResponse(produtos, imagensPorProdutoId);
        }
    }
}