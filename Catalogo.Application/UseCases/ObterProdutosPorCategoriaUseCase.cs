using Catalogo.Domain.Arguments;
using Catalogo.Domain.Arguments.Base;
using Catalogo.Domain.Interfaces;
using Catalogo.Application.Presenters;

namespace Catalogo.Application.UseCases
{
    public class ObterProdutosPorCategoriaUseCase
    {
        private readonly ICatalogoGateway _gateway;
        private readonly IImagemProdutoGateway _imagemGateway;

        public ObterProdutosPorCategoriaUseCase(ICatalogoGateway gateway, IImagemProdutoGateway imagemGateway)
        {
            _gateway = gateway;
            _imagemGateway = imagemGateway;
        }

        public static ObterProdutosPorCategoriaUseCase Create(ICatalogoGateway gateway, IImagemProdutoGateway imagemGateway)
        {
            return new ObterProdutosPorCategoriaUseCase(gateway, imagemGateway);
        }

        public async Task<ResponseBase<ProdutoResponse>> ExecuteAsync(int categoriaId)
        {
            var produtos = await _gateway.ObterProdutosPorCategoriaAsync(categoriaId);
            
            if (produtos == null)
            {
                return new ResponseBase<ProdutoResponse>() { Sucesso = false, Mensagem = "Erro ao obter produtos por categoria", Resultado = [] };
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
