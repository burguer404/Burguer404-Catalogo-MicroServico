using Catalogo.Domain.Arguments;
using Catalogo.Domain.Arguments.Base;
using Catalogo.Domain.Interfaces;

namespace Catalogo.Application.UseCases
{
    public class VisualizarImagemProdutoUseCase
    {
        private readonly IImagemProdutoGateway _imagemGateway;

        public VisualizarImagemProdutoUseCase(IImagemProdutoGateway imagemGateway)
        {
            _imagemGateway = imagemGateway;
        }

        public static VisualizarImagemProdutoUseCase Create(IImagemProdutoGateway imagemGateway)
        {
            return new VisualizarImagemProdutoUseCase(imagemGateway);
        }

        public async Task<ResponseBase<string>> ExecuteAsync(int produtoId)
        {
            var imagem = await _imagemGateway.ObterImagemPorProdutoIdAsync(produtoId);
            
            if (imagem == null || imagem.ImagemByte == null || imagem.ImagemByte.Length == 0)
            {
                return new ResponseBase<string>() 
                { 
                    Sucesso = false, 
                    Mensagem = "Imagem não encontrada", 
                    Resultado = [""] 
                };
            }

            var imagemBase64 = "data:image/png;base64," + Convert.ToBase64String(imagem.ImagemByte);
            
            return new ResponseBase<string>() 
            { 
                Sucesso = true, 
                Mensagem = "Imagem obtida com sucesso", 
                Resultado = [imagemBase64] 
            };
        }
    }
}
