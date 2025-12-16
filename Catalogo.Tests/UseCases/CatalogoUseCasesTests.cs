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
                Preco = 25.50,
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
                Preco = 25.50,
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

        [Fact]
        public async Task ExecuteAsync_DeveCriarImagem_QuandoProdutoCriadoComImagem()
        {
            // Arrange
            var imagemBytes = new byte[] { 1, 2, 3, 4, 5 };
            var request = new ProdutoRequest 
            { 
                Nome = "Hambúrguer",
                Descricao = "Delicioso hambúrguer",
                Preco = 25.50,
                CategoriaId = 1,
                ImagemByte = imagemBytes
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
            imagemGatewayMock.Verify(i => i.CriarImagemAsync(It.Is<ImagemProdutoEntity>(img => 
                img.ProdutoId == produtoEntity.Id && 
                img.ImagemByte == imagemBytes)), Times.Once);
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
                Preco = 30.00,
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
                Preco = 30.00,
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
                Preco = 25.50,
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

        [Fact]
        public async Task ExecuteAsync_DeveAtualizarImagem_QuandoImagemJaExiste()
        {
            // Arrange
            var imagemBytesAntigos = new byte[] { 1, 2, 3 };
            var imagemBytesNovos = new byte[] { 4, 5, 6 };
            var request = new ProdutoRequest 
            { 
                Id = 1,
                Nome = "Hambúrguer Atualizado",
                Descricao = "Hambúrguer ainda mais delicioso",
                Preco = 30.00,
                CategoriaId = 1,
                Status = true,
                ImagemByte = imagemBytesNovos
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

            var imagemExistente = new ImagemProdutoEntity
            {
                ProdutoId = request.Id,
                ImagemByte = imagemBytesAntigos
            };

            var imagemAtualizada = new ImagemProdutoEntity
            {
                ProdutoId = request.Id,
                ImagemByte = imagemBytesNovos
            };
            
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.AtualizarProdutoAsync(It.IsAny<ProdutoEntity>()))
                      .ReturnsAsync(produtoEntity);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            
            imagemGatewayMock.SetupSequence(i => i.ObterImagemPorProdutoIdAsync(request.Id))
                           .ReturnsAsync(imagemExistente)
                           .ReturnsAsync(imagemAtualizada);

            var useCase = new AtualizarProdutoUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            imagemGatewayMock.Verify(i => i.AtualizarImagemAsync(It.Is<ImagemProdutoEntity>(img => 
                img.ProdutoId == request.Id && 
                img.ImagemByte == imagemBytesNovos)), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_DeveCriarImagem_QuandoImagemNaoExiste()
        {
            // Arrange
            var imagemBytes = new byte[] { 1, 2, 3, 4 };
            var request = new ProdutoRequest 
            { 
                Id = 1,
                Nome = "Hambúrguer Atualizado",
                Descricao = "Hambúrguer ainda mais delicioso",
                Preco = 30.00,
                CategoriaId = 1,
                Status = true,
                ImagemByte = imagemBytes
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

            var imagemCriada = new ImagemProdutoEntity
            {
                ProdutoId = request.Id,
                ImagemByte = imagemBytes
            };

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            
            imagemGatewayMock.SetupSequence(i => i.ObterImagemPorProdutoIdAsync(request.Id))
                           .ReturnsAsync((ImagemProdutoEntity?)null)
                           .ReturnsAsync(imagemCriada);

            var useCase = new AtualizarProdutoUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            imagemGatewayMock.Verify(i => i.CriarImagemAsync(It.Is<ImagemProdutoEntity>(img => 
                img.ProdutoId == request.Id && 
                img.ImagemByte == imagemBytes)), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarImagemBase64_QuandoImagemExiste()
        {
            // Arrange
            var imagemBytes = new byte[] { 1, 2, 3, 4 };
            var request = new ProdutoRequest 
            { 
                Id = 1,
                Nome = "Hambúrguer Atualizado",
                Descricao = "Hambúrguer ainda mais delicioso",
                Preco = 30.00,
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

            var imagemExistente = new ImagemProdutoEntity
            {
                ProdutoId = request.Id,
                ImagemByte = imagemBytes
            };
            
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.AtualizarProdutoAsync(It.IsAny<ProdutoEntity>()))
                      .ReturnsAsync(produtoEntity);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(request.Id))
                           .ReturnsAsync(imagemExistente);

            var useCase = new AtualizarProdutoUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Single(result.Resultado);
            var expectedBase64 = "data:image/png;base64," + Convert.ToBase64String(imagemBytes);
            Assert.Equal(expectedBase64, result.Resultado.First().ImagemBase64);
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarImagemBase64Vazia_QuandoImagemNaoExiste()
        {
            // Arrange
            var request = new ProdutoRequest 
            { 
                Id = 1,
                Nome = "Hambúrguer Atualizado",
                Descricao = "Hambúrguer ainda mais delicioso",
                Preco = 30.00,
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
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(request.Id))
                           .ReturnsAsync((ImagemProdutoEntity?)null);

            var useCase = new AtualizarProdutoUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Single(result.Resultado);
            Assert.Empty(result.Resultado.First().ImagemBase64);
        }

        [Fact]
        public async Task ExecuteAsync_NaoDeveAlterarImagem_QuandoNaoInformaImagem()
        {
            // Arrange
            var imagemBytesExistentes = new byte[] { 1, 2, 3, 4 };
            var request = new ProdutoRequest 
            { 
                Id = 1,
                Nome = "Hambúrguer Atualizado",
                Descricao = "Hambúrguer ainda mais delicioso",
                Preco = 30.00,
                CategoriaId = 1,
                Status = true,
                ImagemByte = null
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

            var imagemExistente = new ImagemProdutoEntity
            {
                ProdutoId = request.Id,
                ImagemByte = imagemBytesExistentes
            };
            
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.AtualizarProdutoAsync(It.IsAny<ProdutoEntity>()))
                      .ReturnsAsync(produtoEntity);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(request.Id))
                           .ReturnsAsync(imagemExistente);

            var useCase = new AtualizarProdutoUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            imagemGatewayMock.Verify(i => i.AtualizarImagemAsync(It.IsAny<ImagemProdutoEntity>()), Times.Never);
            imagemGatewayMock.Verify(i => i.CriarImagemAsync(It.IsAny<ImagemProdutoEntity>()), Times.Never);
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

        [Fact]
        public async Task ExecuteAsync_DeveRemoverImagem_QuandoProdutoRemovidoComSucesso()
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
            imagemGatewayMock.Verify(i => i.RemoverImagemAsync(produtoId), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_NaoDeveRemoverImagem_QuandoFalhaAoRemoverProduto()
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
            imagemGatewayMock.Verify(i => i.RemoverImagemAsync(It.IsAny<int>()), Times.Never);
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
                new ProdutoEntity { Nome = "Produto 1", Preco = 10.0 },
                new ProdutoEntity { Nome = "Produto 2", Preco = 20.0 }
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

        [Fact]
        public async Task ExecuteAsync_DeveRetornarProdutosComImagens_QuandoProdutosTemImagens()
        {
            // Arrange
            var imagemBytes = new byte[] { 1, 2, 3, 4 };
            var produtos = new List<ProdutoEntity> 
            { 
                new ProdutoEntity { Id = 1, Nome = "Produto 1", Preco = 10.0 },
                new ProdutoEntity { Id = 2, Nome = "Produto 2", Preco = 20.0 }
            };
            
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ListarProdutosAsync())
                      .ReturnsAsync(produtos);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(1))
                           .ReturnsAsync(new ImagemProdutoEntity { ProdutoId = 1, ImagemByte = imagemBytes });
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(2))
                           .ReturnsAsync(new ImagemProdutoEntity { ProdutoId = 2, ImagemByte = imagemBytes });
            
            var useCase = new ListarProdutosUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Equal(2, result.Resultado.Count());
            var expectedBase64 = "data:image/png;base64," + Convert.ToBase64String(imagemBytes);
            Assert.All(result.Resultado, p => Assert.Equal(expectedBase64, p.ImagemBase64));
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarProdutosSemImagens_QuandoProdutosNaoTemImagens()
        {
            // Arrange
            var produtos = new List<ProdutoEntity> 
            { 
                new ProdutoEntity { Id = 1, Nome = "Produto 1", Preco = 10.0 },
                new ProdutoEntity { Id = 2, Nome = "Produto 2", Preco = 20.0 }
            };
            
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ListarProdutosAsync())
                      .ReturnsAsync(produtos);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((ImagemProdutoEntity?)null);
            
            var useCase = new ListarProdutosUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Equal(2, result.Resultado.Count());
            Assert.All(result.Resultado, p => Assert.Empty(p.ImagemBase64));
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarProdutosComMixDeImagens_QuandoAlgunsTemImagens()
        {
            // Arrange
            var imagemBytes = new byte[] { 1, 2, 3, 4 };
            var produtos = new List<ProdutoEntity> 
            { 
                new ProdutoEntity { Id = 1, Nome = "Produto 1", Preco = 10.0 },
                new ProdutoEntity { Id = 2, Nome = "Produto 2", Preco = 20.0 },
                new ProdutoEntity { Id = 3, Nome = "Produto 3", Preco = 30.0 }
            };
            
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ListarProdutosAsync())
                      .ReturnsAsync(produtos);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(1))
                           .ReturnsAsync(new ImagemProdutoEntity { ProdutoId = 1, ImagemByte = imagemBytes });
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(2))
                           .ReturnsAsync((ImagemProdutoEntity?)null);
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(3))
                           .ReturnsAsync(new ImagemProdutoEntity { ProdutoId = 3, ImagemByte = imagemBytes });
            
            var useCase = new ListarProdutosUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Equal(3, result.Resultado.Count());
            var expectedBase64 = "data:image/png;base64," + Convert.ToBase64String(imagemBytes);
            var produto1 = result.Resultado.First(p => p.Id == 1);
            var produto2 = result.Resultado.First(p => p.Id == 2);
            var produto3 = result.Resultado.First(p => p.Id == 3);
            Assert.Equal(expectedBase64, produto1.ImagemBase64);
            Assert.Empty(produto2.ImagemBase64);
            Assert.Equal(expectedBase64, produto3.ImagemBase64);
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarFalha_QuandoGatewayRetornaNull()
        {
            // Arrange
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ListarProdutosAsync())
                      .ReturnsAsync((List<ProdutoEntity>?)null);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            
            var useCase = new ListarProdutosUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Sucesso);
            Assert.Contains("erro", result.Mensagem.ToLower());
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
                Preco = 15.0
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

        [Fact]
        public async Task ExecuteAsync_DeveRetornarProdutoComImagem_QuandoImagemExiste()
        {
            // Arrange
            var produtoId = 1;
            var imagemBytes = new byte[] { 1, 2, 3, 4 };
            var produto = new ProdutoEntity 
            { 
                Id = produtoId, 
                Nome = "Produto Teste",
                Preco = 15.0
            };
            
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ObterProdutoPorIdAsync(produtoId))
                      .ReturnsAsync(produto);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(produtoId))
                           .ReturnsAsync(new ImagemProdutoEntity { ProdutoId = produtoId, ImagemByte = imagemBytes });
            
            var useCase = new ObterProdutoPorIdUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(produtoId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Single(result.Resultado);
            var expectedBase64 = "data:image/png;base64," + Convert.ToBase64String(imagemBytes);
            Assert.Equal(expectedBase64, result.Resultado.First().ImagemBase64);
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarProdutoSemImagem_QuandoImagemNaoExiste()
        {
            // Arrange
            var produtoId = 1;
            var produto = new ProdutoEntity 
            { 
                Id = produtoId, 
                Nome = "Produto Teste",
                Preco = 15.0
            };
            
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ObterProdutoPorIdAsync(produtoId))
                      .ReturnsAsync(produto);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(produtoId))
                           .ReturnsAsync((ImagemProdutoEntity?)null);
            
            var useCase = new ObterProdutoPorIdUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(produtoId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Single(result.Resultado);
            Assert.Empty(result.Resultado.First().ImagemBase64);
        }

        [Fact]
        public async Task ExecuteAsync_DeveTratarExcecao_QuandoGatewayLancaExcecao()
        {
            // Arrange
            var produtoId = 1;
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ObterProdutoPorIdAsync(produtoId))
                      .ThrowsAsync(new Exception("Erro ao acessar banco de dados"));

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            
            var useCase = new ObterProdutoPorIdUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(produtoId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Sucesso);
            Assert.Equal("Erro ao acessar banco de dados", result.Mensagem);
            Assert.NotNull(result.Resultado);
            Assert.Empty(result.Resultado);
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
                new ProdutoEntity { Nome = "Batata Frita", Status = true, Preco = 8.0 },
                new ProdutoEntity { Nome = "Produto Inativo", Status = false, Preco = 10.0 }
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
                            new ProdutoResponse { Id = 1, Nome = "Hambúrguer", Preco = 25.0, CategoriaId = 1 },
                            new ProdutoResponse { Id = 2, Nome = "Batata Frita", Preco = 8.0, CategoriaId = 1 }
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

        [Fact]
        public async Task ExecuteAsync_DeveRetornarCardapioComProdutosComImagens()
        {
            // Arrange
            var imagemBytes = new byte[] { 1, 2, 3, 4 };
            var cardapio = new CardapioResponse
            {
                Sucesso = true,
                Mensagem = "Cardápio obtido com sucesso",
                TotalProdutos = 2,
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
                            new ProdutoResponse { Id = 1, Nome = "Hambúrguer", Preco = 25.0, CategoriaId = 1 },
                            new ProdutoResponse { Id = 2, Nome = "Batata Frita", Preco = 8.0, CategoriaId = 1 }
                        }
                    }
                }
            };

            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ObterCardapioAsync())
                      .ReturnsAsync(cardapio);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(1))
                           .ReturnsAsync(new ImagemProdutoEntity { ProdutoId = 1, ImagemByte = imagemBytes });
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(2))
                           .ReturnsAsync(new ImagemProdutoEntity { ProdutoId = 2, ImagemByte = imagemBytes });
            
            var useCase = new ObterCardapioUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Single(result.Resultado);
            var categoria = result.Resultado.First();
            var expectedBase64 = "data:image/png;base64," + Convert.ToBase64String(imagemBytes);
            Assert.All(categoria.Categorias.SelectMany(c => c.Produtos), p => Assert.Equal(expectedBase64, p.ImagemBase64));
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarCardapioComProdutosSemImagens()
        {
            // Arrange
            var cardapio = new CardapioResponse
            {
                Sucesso = true,
                Mensagem = "Cardápio obtido com sucesso",
                TotalProdutos = 2,
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
                            new ProdutoResponse { Id = 1, Nome = "Hambúrguer", Preco = 25.0, CategoriaId = 1 },
                            new ProdutoResponse { Id = 2, Nome = "Batata Frita", Preco = 8.0, CategoriaId = 1 }
                        }
                    }
                }
            };

            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ObterCardapioAsync())
                      .ReturnsAsync(cardapio);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((ImagemProdutoEntity?)null);
            
            var useCase = new ObterCardapioUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Single(result.Resultado);
            var categoria = result.Resultado.First();
            Assert.All(categoria.Categorias.SelectMany(c => c.Produtos), p => Assert.Empty(p.ImagemBase64));
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarCardapioComMixDeImagens()
        {
            // Arrange
            var imagemBytes = new byte[] { 1, 2, 3, 4 };
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
                            new ProdutoResponse { Id = 1, Nome = "Hambúrguer", Preco = 25.0, CategoriaId = 1 },
                            new ProdutoResponse { Id = 2, Nome = "Batata Frita", Preco = 8.0, CategoriaId = 1 },
                            new ProdutoResponse { Id = 3, Nome = "Refrigerante", Preco = 5.0, CategoriaId = 1 }
                        }
                    }
                }
            };

            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ObterCardapioAsync())
                      .ReturnsAsync(cardapio);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(1))
                           .ReturnsAsync(new ImagemProdutoEntity { ProdutoId = 1, ImagemByte = imagemBytes });
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(2))
                           .ReturnsAsync((ImagemProdutoEntity?)null);
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(3))
                           .ReturnsAsync(new ImagemProdutoEntity { ProdutoId = 3, ImagemByte = imagemBytes });
            
            var useCase = new ObterCardapioUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Single(result.Resultado);
            var categoria = result.Resultado.First();
            var produtos = categoria.Categorias.SelectMany(c => c.Produtos).ToList();
            var expectedBase64 = "data:image/png;base64," + Convert.ToBase64String(imagemBytes);
            Assert.Equal(expectedBase64, produtos.First(p => p.Id == 1).ImagemBase64);
            Assert.Empty(produtos.First(p => p.Id == 2).ImagemBase64);
            Assert.Equal(expectedBase64, produtos.First(p => p.Id == 3).ImagemBase64);
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarCardapioVazio_QuandoNaoHaCategorias()
        {
            // Arrange
            var cardapio = new CardapioResponse
            {
                Sucesso = true,
                Mensagem = "Cardápio obtido com sucesso",
                TotalProdutos = 0,
                DataAtualizacao = DateTime.Now,
                Categorias = new List<CategoriaResponse>()
            };

            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ObterCardapioAsync())
                      .ReturnsAsync(cardapio);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            
            var useCase = new ObterCardapioUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Single(result.Resultado);
            Assert.Empty(result.Resultado.First().Categorias);
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarCardapioComCategoriasVazias_QuandoNaoHaProdutos()
        {
            // Arrange
            var cardapio = new CardapioResponse
            {
                Sucesso = true,
                Mensagem = "Cardápio obtido com sucesso",
                TotalProdutos = 0,
                DataAtualizacao = DateTime.Now,
                Categorias = new List<CategoriaResponse>
                {
                    new CategoriaResponse
                    {
                        Id = 1,
                        Descricao = "Lanches",
                        Ativa = true,
                        Produtos = new List<ProdutoResponse>()
                    }
                }
            };

            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ObterCardapioAsync())
                      .ReturnsAsync(cardapio);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            
            var useCase = new ObterCardapioUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Single(result.Resultado);
            var categoria = result.Resultado.First();
            Assert.Single(categoria.Categorias);
            Assert.Empty(categoria.Categorias.First().Produtos);
        }

        [Fact]
        public async Task ExecuteAsync_DeveTratarExcecao_QuandoGatewayLancaExcecao()
        {
            // Arrange
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ObterCardapioAsync())
                      .ThrowsAsync(new Exception("Erro ao acessar banco de dados"));

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            
            var useCase = new ObterCardapioUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Sucesso);
            Assert.Equal("Erro ao acessar banco de dados", result.Mensagem);
            Assert.NotNull(result.Resultado);
            Assert.Empty(result.Resultado);
        }
    }

    public class ObterProdutosPorCategoriaUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_DeveRetornarProdutos_QuandoCategoriaTemProdutos()
        {
            // Arrange
            var categoriaId = 1;
            var produtos = new List<ProdutoEntity> 
            { 
                new ProdutoEntity { Id = 1, Nome = "Produto 1", Preco = 10.0, CategoriaId = categoriaId },
                new ProdutoEntity { Id = 2, Nome = "Produto 2", Preco = 20.0, CategoriaId = categoriaId }
            };
            
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ObterProdutosPorCategoriaAsync(categoriaId))
                      .ReturnsAsync(produtos);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((ImagemProdutoEntity?)null);
            
            var useCase = new ObterProdutosPorCategoriaUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(categoriaId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Equal(2, result.Resultado.Count());
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarListaVazia_QuandoCategoriaNaoTemProdutos()
        {
            // Arrange
            var categoriaId = 1;
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ObterProdutosPorCategoriaAsync(categoriaId))
                      .ReturnsAsync(new List<ProdutoEntity>());

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            
            var useCase = new ObterProdutosPorCategoriaUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(categoriaId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Empty(result.Resultado);
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarProdutosComImagens_QuandoProdutosTemImagens()
        {
            // Arrange
            var categoriaId = 1;
            var imagemBytes = new byte[] { 1, 2, 3, 4 };
            var produtos = new List<ProdutoEntity> 
            { 
                new ProdutoEntity { Id = 1, Nome = "Produto 1", Preco = 10.0, CategoriaId = categoriaId },
                new ProdutoEntity { Id = 2, Nome = "Produto 2", Preco = 20.0, CategoriaId = categoriaId }
            };
            
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ObterProdutosPorCategoriaAsync(categoriaId))
                      .ReturnsAsync(produtos);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(1))
                           .ReturnsAsync(new ImagemProdutoEntity { ProdutoId = 1, ImagemByte = imagemBytes });
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(2))
                           .ReturnsAsync(new ImagemProdutoEntity { ProdutoId = 2, ImagemByte = imagemBytes });
            
            var useCase = new ObterProdutosPorCategoriaUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(categoriaId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Equal(2, result.Resultado.Count());
            var expectedBase64 = "data:image/png;base64," + Convert.ToBase64String(imagemBytes);
            Assert.All(result.Resultado, p => Assert.Equal(expectedBase64, p.ImagemBase64));
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarProdutosSemImagens_QuandoProdutosNaoTemImagens()
        {
            // Arrange
            var categoriaId = 1;
            var produtos = new List<ProdutoEntity> 
            { 
                new ProdutoEntity { Id = 1, Nome = "Produto 1", Preco = 10.0, CategoriaId = categoriaId },
                new ProdutoEntity { Id = 2, Nome = "Produto 2", Preco = 20.0, CategoriaId = categoriaId }
            };
            
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ObterProdutosPorCategoriaAsync(categoriaId))
                      .ReturnsAsync(produtos);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((ImagemProdutoEntity?)null);
            
            var useCase = new ObterProdutosPorCategoriaUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(categoriaId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Equal(2, result.Resultado.Count());
            Assert.All(result.Resultado, p => Assert.Empty(p.ImagemBase64));
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarProdutosComMixDeImagens_QuandoAlgunsTemImagens()
        {
            // Arrange
            var categoriaId = 1;
            var imagemBytes = new byte[] { 1, 2, 3, 4 };
            var produtos = new List<ProdutoEntity> 
            { 
                new ProdutoEntity { Id = 1, Nome = "Produto 1", Preco = 10.0, CategoriaId = categoriaId },
                new ProdutoEntity { Id = 2, Nome = "Produto 2", Preco = 20.0, CategoriaId = categoriaId },
                new ProdutoEntity { Id = 3, Nome = "Produto 3", Preco = 30.0, CategoriaId = categoriaId }
            };
            
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ObterProdutosPorCategoriaAsync(categoriaId))
                      .ReturnsAsync(produtos);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(1))
                           .ReturnsAsync(new ImagemProdutoEntity { ProdutoId = 1, ImagemByte = imagemBytes });
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(2))
                           .ReturnsAsync((ImagemProdutoEntity?)null);
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(3))
                           .ReturnsAsync(new ImagemProdutoEntity { ProdutoId = 3, ImagemByte = imagemBytes });
            
            var useCase = new ObterProdutosPorCategoriaUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(categoriaId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.NotNull(result.Resultado);
            Assert.Equal(3, result.Resultado.Count());
            var expectedBase64 = "data:image/png;base64," + Convert.ToBase64String(imagemBytes);
            var produto1 = result.Resultado.First(p => p.Id == 1);
            var produto2 = result.Resultado.First(p => p.Id == 2);
            var produto3 = result.Resultado.First(p => p.Id == 3);
            Assert.Equal(expectedBase64, produto1.ImagemBase64);
            Assert.Empty(produto2.ImagemBase64);
            Assert.Equal(expectedBase64, produto3.ImagemBase64);
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarFalha_QuandoGatewayRetornaNull()
        {
            // Arrange
            var categoriaId = 1;
            var gatewayMock = new Mock<ICatalogoGateway>();
            gatewayMock.Setup(g => g.ObterProdutosPorCategoriaAsync(categoriaId))
                      .ReturnsAsync((List<ProdutoEntity>?)null);

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            
            var useCase = new ObterProdutosPorCategoriaUseCase(gatewayMock.Object, imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(categoriaId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Sucesso);
            Assert.Contains("erro", result.Mensagem.ToLower());
            Assert.NotNull(result.Resultado);
            Assert.Empty(result.Resultado);
        }
    }

    public class VisualizarImagemProdutoUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_DeveRetornarImagemBase64_QuandoImagemExiste()
        {
            // Arrange
            var produtoId = 1;
            var imagemBytes = new byte[] { 1, 2, 3, 4, 5 };
            var imagem = new ImagemProdutoEntity
            {
                ProdutoId = produtoId,
                ImagemByte = imagemBytes
            };

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(produtoId))
                           .ReturnsAsync(imagem);

            var useCase = new VisualizarImagemProdutoUseCase(imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(produtoId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            Assert.Contains("sucesso", result.Mensagem.ToLower());
            Assert.NotNull(result.Resultado);
            Assert.Single(result.Resultado);
            var expectedBase64 = "data:image/png;base64," + Convert.ToBase64String(imagemBytes);
            Assert.Equal(expectedBase64, result.Resultado.First());
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarFalha_QuandoImagemNaoExiste()
        {
            // Arrange
            var produtoId = 1;
            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(produtoId))
                           .ReturnsAsync((ImagemProdutoEntity?)null);

            var useCase = new VisualizarImagemProdutoUseCase(imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(produtoId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Sucesso);
            Assert.Contains("não encontrada", result.Mensagem.ToLower());
            Assert.NotNull(result.Resultado);
            Assert.Single(result.Resultado);
            Assert.Equal("", result.Resultado.First());
        }

        [Fact]
        public async Task ExecuteAsync_DeveRetornarFalha_QuandoImagemByteEstaVazio()
        {
            // Arrange
            var produtoId = 1;
            var imagem = new ImagemProdutoEntity
            {
                ProdutoId = produtoId,
                ImagemByte = Array.Empty<byte>()
            };

            var imagemGatewayMock = new Mock<IImagemProdutoGateway>();
            imagemGatewayMock.Setup(i => i.ObterImagemPorProdutoIdAsync(produtoId))
                           .ReturnsAsync(imagem);

            var useCase = new VisualizarImagemProdutoUseCase(imagemGatewayMock.Object);

            // Act
            var result = await useCase.ExecuteAsync(produtoId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Sucesso);
            Assert.Contains("não encontrada", result.Mensagem.ToLower());
            Assert.NotNull(result.Resultado);
            Assert.Single(result.Resultado);
            Assert.Equal("", result.Resultado.First());
        }
    }
}