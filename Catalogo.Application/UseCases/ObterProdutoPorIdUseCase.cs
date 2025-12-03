using Catalogo.Domain.Arguments;
using Catalogo.Domain.Arguments.Base;
using Catalogo.Domain.Interfaces;
using Catalogo.Application.Presenters;

namespace Catalogo.Application.UseCases
{
    public class ObterProdutoPorIdUseCase
    {
        private readonly ICatalogoGateway _catalogoGateway;
        private readonly IImagemProdutoGateway _imagemGateway;

        public ObterProdutoPorIdUseCase(ICatalogoGateway catalogoGateway, IImagemProdutoGateway imagemGateway)
        {
            _catalogoGateway = catalogoGateway;
            _imagemGateway = imagemGateway;
        }

        public async Task<ResponseBase<ProdutoResponse>> ExecuteAsync(int id)
        {
            var response = new ResponseBase<ProdutoResponse>();
            try
            {
                var produto = await _catalogoGateway.ObterProdutoPorIdAsync(id);
                if (produto == null)
                {
                    response.Sucesso = false;
                    response.Mensagem = "Produto não encontrado!";
                    response.Resultado = new List<ProdutoResponse>();
                    return response;
                }

                var imagem = await _imagemGateway.ObterImagemPorProdutoIdAsync(id);
                var imagemBase64 = string.Empty;
                if (imagem != null && imagem.ImagemByte != null && imagem.ImagemByte.Length > 0)
                {
                    imagemBase64 = "data:image/png;base64," + Convert.ToBase64String(imagem.ImagemByte);
                }

                response = CatalogoPresenter.ObterProdutoResponse(produto, imagemBase64);
                return response;
            }
            catch (Exception ex)
            {
                response.Sucesso = false;
                response.Mensagem = ex.Message;
                response.Resultado = new List<ProdutoResponse>();
                return response;
            }
        }
    }
}
