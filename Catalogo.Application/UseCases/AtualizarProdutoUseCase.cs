using Catalogo.Application.Presenters;
using Catalogo.Domain.Arguments;
using Catalogo.Domain.Arguments.Base;
using Catalogo.Domain.Entities;
using Catalogo.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Catalogo.Application.UseCases
{
    public class AtualizarProdutoUseCase
    {
        private readonly ICatalogoGateway _gateway;
        private readonly IImagemProdutoGateway _imagemGateway;

        public AtualizarProdutoUseCase(ICatalogoGateway gateway, IImagemProdutoGateway imagemGateway)
        {
            _gateway = gateway;
            _imagemGateway = imagemGateway;
        }

        public static AtualizarProdutoUseCase Create(ICatalogoGateway gateway, IImagemProdutoGateway imagemGateway)
        {
            return new AtualizarProdutoUseCase(gateway, imagemGateway);
        }

        public async Task<ResponseBase<ProdutoResponse>> ExecuteAsync(ProdutoRequest request)
        {
            var produto = new ProdutoEntity
            {
                Id = request.Id,
                Nome = request.Nome,
                Descricao = request.Descricao,
                Preco = request.Preco,
                CategoriaId = request.CategoriaId,
                Status = request.Status ?? true,
                DataAtualizacao = DateTime.Now
            };

            var produtoAtualizado = await _gateway.AtualizarProdutoAsync(produto);
            
            if (produtoAtualizado == null)
            {
                return new ResponseBase<ProdutoResponse>() { Sucesso = false, Mensagem = "Erro ao atualizar produto", Resultado = [] };
            }

            if (request.Imagem != null && request.Imagem.Length > 0)
            {
                var imagemExistente = await _imagemGateway.ObterImagemPorProdutoIdAsync(produtoAtualizado.Id);
                
                if (imagemExistente != null)
                {
                    imagemExistente.ImagemByte = await ConverterMemoryStream(request.Imagem) ?? [];
                    await _imagemGateway.AtualizarImagemAsync(imagemExistente);
                }
                else
                {
                    var imagemProduto = new ImagemProdutoEntity
                    {
                        ProdutoId = produtoAtualizado.Id,
                        ImagemByte = await ConverterMemoryStream(request.Imagem) ?? []
                    };
                    await _imagemGateway.CriarImagemAsync(imagemProduto);
                }
            }

            var imagem = await _imagemGateway.ObterImagemPorProdutoIdAsync(produtoAtualizado.Id);
            var imagemBase64 = string.Empty;
            if (imagem != null && imagem.ImagemByte != null && imagem.ImagemByte.Length > 0)
            {
                imagemBase64 = "data:image/png;base64," + Convert.ToBase64String(imagem.ImagemByte);
            }

            return CatalogoPresenter.ObterProdutoResponse(produtoAtualizado, imagemBase64);
        }

        public async Task<byte[]?> ConverterMemoryStream(IFormFile? imagem)
        {
            if (imagem == null || imagem.Length == 0)
                return null;

            using var ms = new MemoryStream();
            await imagem.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}
