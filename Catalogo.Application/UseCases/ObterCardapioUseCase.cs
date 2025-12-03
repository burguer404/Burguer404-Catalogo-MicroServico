using Catalogo.Domain.Arguments;
using Catalogo.Domain.Arguments.Base;
using Catalogo.Domain.Interfaces;

namespace Catalogo.Application.UseCases
{
    public class ObterCardapioUseCase
    {
        private readonly ICatalogoGateway _catalogoGateway;
        private readonly IImagemProdutoGateway _imagemGateway;

        public ObterCardapioUseCase(ICatalogoGateway catalogoGateway, IImagemProdutoGateway imagemGateway)
        {
            _catalogoGateway = catalogoGateway;
            _imagemGateway = imagemGateway;
        }

        public async Task<ResponseBase<CardapioResponse>> ExecuteAsync()
        {
            var response = new ResponseBase<CardapioResponse>();
            try
            {
                var cardapio = await _catalogoGateway.ObterCardapioAsync();
                
                if (cardapio.Categorias != null)
                {
                    foreach (var categoria in cardapio.Categorias)
                    {
                        if (categoria.Produtos != null)
                        {
                            foreach (var produto in categoria.Produtos)
                            {
                                var imagem = await _imagemGateway.ObterImagemPorProdutoIdAsync(produto.Id);
                                if (imagem != null && imagem.ImagemByte != null && imagem.ImagemByte.Length > 0)
                                {
                                    produto.ImagemBase64 = "data:image/png;base64," + Convert.ToBase64String(imagem.ImagemByte);
                                }
                            }
                        }
                    }
                }
                
                response.Sucesso = true;
                response.Mensagem = "Cardápio obtido com sucesso";
                response.Resultado = [cardapio];
                return response;
            }
            catch (Exception ex)
            {
                response.Sucesso = false;
                response.Mensagem = ex.Message;
                response.Resultado = new List<CardapioResponse>();
                return response;
            }
        }
    }
}
