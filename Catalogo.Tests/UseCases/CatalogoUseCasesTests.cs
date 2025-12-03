using Catalogo.Application.UseCases;
using Catalogo.Domain.Entities;
using Catalogo.Domain.Interfaces;
using Catalogo.Domain.Arguments.Base;
using Catalogo.Domain.Arguments;
using Moq;
using Xunit;

namespace Catalogo.Tests.UseCases
{
    public class CriarProdutoUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_DeveRetornarProduto_QuandoDadosValidos()
        {
            // Arrange
            var request = new ProdutoRequest 
            { 
                Nome = "Hambúrguer",
                Descricao = "Delicioso hambúrguer",
                Preco = 25.50m,
                CategoriaId = 1
            };
            
            var produtoEntity = new ProdutoEntity 
            { 
                Id = 1,
                Nome = request.Nome,
                Descricao = request.Descricao,
                Preco = request.Preco,
                CategoriaId = request.CategoriaId,
                Status = true
            };
            
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.CriarProdutoAsync(It.IsAny<ProdutoEntity>()))
                      .ReturnsAsync(produtoEntity);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();

            var useCase = new CriarProdutoUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Single(result.Resultado);
            Assert.Equal(request.Nome, result.Resultado.First().Nome);
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarFalha_QuandoErroAoCriar()
        {
            // Arrange
            var request = new ProdutoRequest 
            { 
                Nome = "Hambúrguer",
                Descricao = "Delicioso hambúrguer",
                Preco = 25.50m,
                CategoriaId = 1
            };
            
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.CriarProdutoAsync(It.IsAny<ProdutoEntity>()))
                      .ReturnsAsync((ProdutoEntity)null!);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();

            var useCase = new CriarProdutoUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Sucesso);
            Assert.Contains("erro", result.Mensagem.ToLower());
        }

        [Fact]
        public async Task ConverterMemoryStream_DeveRetornarBytes_QuandoImagemExiste()
        {
            // Arrange
            var request = new ProdutoRequest 
            { 
                Nome = "Hambúrguer",
                ImagemByte = new byte[] { 1, 2, 3, 4 }
            };
            
            var useCase = new CriarProdutoUseCase(Mock.Of<ICatalogoGateway>(), Mock.Of<IImagemProdutoGateway>());

            // Act
            var result = await useCase.ConverterMemoryStream(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(new byte[] { 1, 2, 3, 4 }, result);
        }

        [Fact]
        public async Task ConverterMemoryStream_DeveRetornarNull_QuandoImagemNaoExiste()
        {
            // Arrange
            var request = new ProdutoRequest 
            { 
                Nome = "Hambúrguer",
                ImagemByte = null
            };
            
            var useCase = new CriarProdutoUseCase(Mock.Of<ICatalogoGateway>(), Mock.Of<IImagemProdutoGateway>());

            // Act
            var result = await useCase.ConverterMemoryStream(request);

            // Assert
            Assert.Null(result);
        }
    }

    public class AtualizarProdutoUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_DeveRetornarProduto_QuandoAtualizacaoSucesso()
        {
            // Arrange
            var request = new ProdutoRequest 
            { 
                Id = 1,
                Nome = "Hambúrguer Atualizado",
                Descricao = "Hambúrguer ainda mais delicioso",
                Preco = 30.00m,
                CategoriaId = 1,
                Status = true
            };
            
            var produtoEntity = new ProdutoEntity 
            { 
                Id = request.Id,
                Nome = request.Nome,
                Descricao = request.Descricao,
                Preco = request.Preco,
                CategoriaId = request.CategoriaId,
                Status = request.Status.Value
            };
            
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.AtualizarProdutoAsync(It.IsAny<ProdutoEntity>()))
                      .ReturnsAsync(produtoEntity);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((Catalogo.Domain.Entities.ImagemProdutoEntity?)null);

            var useCase = new AtualizarProdutoUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Single(result.Resultado);
            Assert.Equal(request.Nome, result.Resultado.First().Nome);
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarFalha_QuandoErroAoAtualizar()
        {
            // Arrange
            var request = new ProdutoRequest 
            { 
                Id = 1,
                Nome = "Hambúrguer Atualizado",
                Descricao = "Hambúrguer ainda mais delicioso",
                Preco = 30.00m,
                CategoriaId = 1
            };
            
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.AtualizarProdutoAsync(It.IsAny<ProdutoEntity>()))
                      .ReturnsAsync((ProdutoEntity)null!);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();

            var useCase = new AtualizarProdutoUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Sucesso);
            Assert.Contains("erro", result.Mensagem.ToLower());
        }

        [Fact]
        public async Task ExecuteAsync_DeveUsarStatusPadrao_QuandoStatusNaoInformado()
        {
            // Arrange
            var request = new ProdutoRequest 
            { 
                Id = 1,
                Nome = "Hambúrguer",
                Descricao = "Delicioso hambúrguer",
                Preco = 25.50m,
                CategoriaId = 1,
                Status = null
            };
            
            var produtoEntity = new ProdutoEntity 
            { 
                Id = request.Id,
                Nome = request.Nome,
                Status = true
            };
            
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.AtualizarProdutoAsync(It.IsAny<ProdutoEntity>()))
                      .ReturnsAsync(produtoEntity);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((Catalogo.Domain.Entities.ImagemProdutoEntity?)null);

            var useCase = new AtualizarProdutoUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
        }
    }

    public class RemoverProdutoUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_DeveRetornarSucesso_QuandoProdutoRemovido()
        {
            // Arrange
            var produtoId = 1;
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.RemoverProdutoAsync(produtoId))
                      .ReturnsAsync(true);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.RemoverImagemAsync(produtoId))
                           .ReturnsAsync(true);

            var useCase = new RemoverProdutoUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(produtoId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.Contains("sucesso", result.Mensagem.ToLower());
            Assert.Single(result.Resultado);
            Assert.True(result.Resultado.First());
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarFalha_QuandoErroAoRemover()
        {
            // Arrange
            var produtoId = 1;
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.RemoverProdutoAsync(produtoId))
                      .ReturnsAsync(false);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();

            var useCase = new RemoverProdutoUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(produtoId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Sucesso);
            Assert.Contains("erro", result.Mensagem.ToLower());
            Assert.Single(result.Resultado);
            Assert.False(result.Resultado.First());
        }
    }

    public class ListarProdutosUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_DeveRetornarListaDeProdutos()
        {
            // Arrange
            var produtos = new List<ProdutoEntity> 
            { 
                new ProdutoEntity { Nome = "Produto 1", Preco = 10.0m },
                new ProdutoEntity { Nome = "Produto 2", Preco = 20.0m }
            };
            
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ListarProdutosAsync())
                      .ReturnsAsync(produtos);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((Catalogo.Domain.Entities.ImagemProdutoEntity?)null);
            
            var useCase = new ListarProdutosUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Equal(2, result.Resultado.Count());
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarListaVazia_QuandoNaoHaProdutos()
        {
            // Arrange
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ListarProdutosAsync())
                      .ReturnsAsync(new List<ProdutoEntity>());

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            
            var useCase = new ListarProdutosUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Empty(result.Resultado);
        }
    }

    public class ObterProdutoPorIdUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_DeveRetornarProduto_QuandoIdValido()
        {
            // Arrange
            var produtoId = 1;
            var produto = new ProdutoEntity 
            { 
                Id = produtoId, 
                Nome = "Produto Teste",
                Preco = 15.0m
            };
            
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ObterProdutoPorIdAsync(produtoId))
                      .ReturnsAsync(produto);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(produtoId))
                           .ReturnsAsync((Catalogo.Domain.Entities.ImagemProdutoEntity?)null);
            
            var useCase = new ObterProdutoPorIdUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(produtoId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Equal(produtoId, result.Resultado.First().Id);
            Assert.Equal("Produto Teste", result.Resultado.First().Nome);
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarFalha_QuandoProdutoNaoExiste()
        {
            // Arrange
            var produtoId = 999;
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ObterProdutoPorIdAsync(produtoId))
                      .ReturnsAsync((ProdutoEntity)null!);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            
            var useCase = new ObterProdutoPorIdUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(produtoId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Sucesso);
            Assert.Contains("não encontrado", result.Mensagem.ToLower());
        }
    }

    public class ObterCardapioUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_DeveRetornarCardapioComProdutosAtivos()
        {
            // Arrange
            var produtos = new List<ProdutoEntity> 
            { 
                new ProdutoEntity { Nome = "Hambúrguer", Status = true, Preco = 25.0m },
                new ProdutoEntity { Nome = "Batata Frita", Status = true, Preco = 8.0m },
                new ProdutoEntity { Nome = "Produto Inativo", Status = false, Preco = 10.0m }
            };
            
            var cardapio = new CardapioResponse
            {
                Sucesso = true,
                Mensagem = "Cardápio obtido com sucesso",
                TotalProdutos = 3,
                DataAtualizacao = DateTime.Now,
                Categorias = new List<CategoriaResponse>
                {
                    new CategoriaResponse
                    {
                        Id = 1,
                        Descricao = "Lanches",
                        Ativa = true,
                        Produtos = new List<ProdutoResponse>
                        {
                            new ProdutoResponse { Id = 1, Nome = "Hambúrguer", Preco = 25.0m, CategoriaId = 1 },
                            new ProdutoResponse { Id = 2, Nome = "Batata Frita", Preco = 8.0m, CategoriaId = 1 }
                        }
                    }
                }
            };

            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ObterCardapioAsync())
                      .ReturnsAsync(cardapio);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((Catalogo.Domain.Entities.ImagemProdutoEntity?)null);
            
            var useCase = new ObterCardapioUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Single(result.Resultado);
            Assert.NotNull(result.Resultado.First());
        }
    }
}