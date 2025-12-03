using Catalogo.Domain.Arguments;
using Catalogo.Domain.Arguments.Base;
using Catalogo.Domain.Interfaces;

namespace Catalogo.Application.UseCases
{
    public class RemoverProdutoUseCase
    {
        private readonly ICatalogoGateway _gateway;
        private readonly IImagemProdutoGateway _imagemGateway;

        public RemoverProdutoUseCase(ICatalogoGateway gateway, IImagemProdutoGateway imagemGateway)
        {
            _gateway = gateway;
            _imagemGateway = imagemGateway;
        }

        public static RemoverProdutoUseCase Create(ICatalogoGateway gateway, IImagemProdutoGateway imagemGateway)
        {
            return new RemoverProdutoUseCase(gateway, imagemGateway);
        }

        public async Task<ResponseBase<bool>> ExecuteAsync(int produtoId)
        {
            var sucesso = await _gateway.RemoverProdutoAsync(produtoId);
            
            if (sucesso)
            {
                await _imagemGateway.RemoverImagemAsync(produtoId);
            }
            
            return new ResponseBase<bool>() 
            { 
                Sucesso = sucesso, 
                Mensagem = sucesso ? "Produto removido com sucesso" : "Erro ao remover produto", 
                Resultado = [sucesso] 
            };
        }
    }
}
