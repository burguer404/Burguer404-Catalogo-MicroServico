using Catalogo.Application.Presenters;
using Catalogo.Domain.Arguments;
using Catalogo.Domain.Arguments.Base;
using Catalogo.Domain.Entities;
using Catalogo.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Catalogo.Application.UseCases
{
    public class CriarProdutoUseCase
    {
        private readonly ICatalogoGateway _gateway;
        private readonly IImagemProdutoGateway _imagemGateway;

        public CriarProdutoUseCase(ICatalogoGateway gateway, IImagemProdutoGateway imagemGateway)
        {
            _gateway = gateway;
            _imagemGateway = imagemGateway;
        }

        public static CriarProdutoUseCase Create(ICatalogoGateway gateway, IImagemProdutoGateway imagemGateway)
        {
            return new CriarProdutoUseCase(gateway, imagemGateway);
        }

        public async Task<ResponseBase<ProdutoResponse>> ExecuteAsync(ProdutoRequest request, byte[] imagem)
        {
            var produto = new ProdutoEntity
            {
                Nome = request.Nome,
                Descricao = request.Descricao,
                Preco = request.Preco,
                CategoriaId = request.CategoriaId,
                Status = true,
                DataCriacao = DateTime.Now,
                DataAtualizacao = DateTime.Now
            };

            var produtoCriado = await _gateway.CriarProdutoAsync(produto);
            
            if (produtoCriado == null)
            {
                return new ResponseBase<ProdutoResponse>() { Sucesso = false, Mensagem = "Erro ao criar produto", Resultado = [] };
            }

            if (imagem != null && imagem.Length > 0)
            {
                var imagemProduto = new ImagemProdutoEntity
                {
                    ProdutoId = produtoCriado.Id,
                    ImagemByte = imagem
                };

                await _imagemGateway.CriarImagemAsync(imagemProduto);
            }

            return CatalogoPresenter.ObterProdutoResponse(produtoCriado);
        }

        
    }
}