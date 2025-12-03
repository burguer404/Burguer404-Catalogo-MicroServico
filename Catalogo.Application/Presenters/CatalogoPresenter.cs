using Catalogo.Domain.Arguments;
using Catalogo.Domain.Arguments.Base;
using Catalogo.Domain.Entities;

namespace Catalogo.Application.Presenters
{
    public static class CatalogoPresenter
    {
        public static ResponseBase<ProdutoResponse> ObterListaProdutoResponse(List<ProdutoEntity> produtos, Dictionary<int, string> imagensPorProdutoId) 
        {
            var listaProdutoMap = new List<ProdutoResponse>();

            foreach (var produto in produtos)
            {
                var produtoMap = new ProdutoResponse() 
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Descricao = produto.Descricao,
                    Preco = produto.Preco,
                    CategoriaId = produto.CategoriaId,
                    CategoriaDescricao = produto.Categoria?.Descricao ?? string.Empty,
                    ImagemBase64 = imagensPorProdutoId.ContainsKey(produto.Id) ? imagensPorProdutoId[produto.Id] : string.Empty,
                    Status = produto.Status,
                    DataCriacao = produto.DataCriacao,
                    DataAtualizacao = produto.DataAtualizacao
                };

                listaProdutoMap.Add(produtoMap);
            }

            return new ResponseBase<ProdutoResponse>() { Sucesso = true, Mensagem = "Produtos listados com sucesso", Resultado = listaProdutoMap };
        }

        public static ResponseBase<ProdutoResponse> ObterProdutoResponse(ProdutoEntity produto, string imagemBase64 = "") 
        {
            var produtoResponse = new ProdutoResponse()
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                CategoriaId = produto.CategoriaId,
                CategoriaDescricao = produto.Categoria?.Descricao ?? string.Empty,
                ImagemBase64 = imagemBase64,
                Status = produto.Status,
                DataCriacao = produto.DataCriacao,
                DataAtualizacao = produto.DataAtualizacao
            };

            var response = new ResponseBase<ProdutoResponse>()
            {
                Sucesso = true,
                Mensagem = "Produto encontrado com sucesso!",
                Resultado = [produtoResponse]
            };

            return response;
        }

        public static ResponseBase<CategoriaResponse> ObterListaCategoriaResponse(List<CategoriaEntity> categorias) 
        {
            var listaCategoriaMap = new List<CategoriaResponse>();

            foreach (var categoria in categorias)
            {
                var categoriaMap = new CategoriaResponse() 
                {
                    Id = categoria.Id,
                    Descricao = categoria.Descricao,
                    Ativa = categoria.Ativa
                };

                listaCategoriaMap.Add(categoriaMap);
            }

            return new ResponseBase<CategoriaResponse>() { Sucesso = true, Mensagem = "Categorias listadas com sucesso", Resultado = listaCategoriaMap };
        }

        public static ResponseBase<CardapioResponse> ObterCardapioResponse(List<ProdutoResponse> produtos)
        {
            var cardapio = new CardapioResponse
            {
                Sucesso = true,
                Mensagem = "Cardápio obtido com sucesso",
                TotalProdutos = produtos.Count,
                DataAtualizacao = DateTime.Now
            };

            var produtosPorCategoria = produtos.GroupBy(p => p.CategoriaId).ToList();

            foreach (var grupo in produtosPorCategoria)
            {
                var categoriaResponse = new CategoriaResponse
                {
                    Id = grupo.Key,
                    Descricao = grupo.First().CategoriaDescricao,
                    Ativa = true,
                    Produtos = grupo.ToList()
                };

                cardapio.Categorias.Add(categoriaResponse);
            }

            return new ResponseBase<CardapioResponse>() { Sucesso = true, Mensagem = "Cardápio obtido com sucesso", Resultado = [cardapio] };
        }
    }
}
