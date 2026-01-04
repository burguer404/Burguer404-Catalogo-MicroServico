# Burguer404 - Microserviço de Catálogo

Microserviço responsável pelo gerenciamento de produtos e cardápio da lanchonete **Burguer404**. Permite o cadastro, edição, remoção e listagem de produtos, organizados por categorias.


## Tecnologias Utilizadas

- **.NET 8.0**: Framework principal.
- **Entity Framework Core 8.0**: ORM para SQL Server.
- **SQL Server**: Banco de dados principal (produtos, categorias).
- **MongoDB**: Banco de dados NoSQL (armazenamento de imagens/detalhes extras).
- **Docker**: Containerização.
- **Kubernetes (AKS)**: Orquestração.
- **Terraform**: Infraestrutura como código.

## Arquitetura

O projeto segue a **Clean Architecture**:

- **Catalogo.Api**: API REST e configurações.
- **Catalogo.Application**: Regras de negócio.
  - *Use Cases*: CriarProduto, ListarProdutos, ObterCardapio, ObterProdutoPorId.
- **Catalogo.Domain**: Entidades (Produto, Categoria, ImagemProduto).
- **Catalogo.Infrastructure**: Implementação de repositórios SQL e MongoDB.
- **Catalogo.Tests**: Testes unitários.

## Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop)
- SQL Server
- MongoDB

## Como Executar Localmente

1. **Clone o repositório**
   ```bash
   git clone https://github.com/burguer404/Burguer404-Catalogo-MicroServico.git
   cd Burguer404-Catalogo-MicroServico
   ```

2. **Configure as Conexões**
   No arquivo `Catalogo.Api/appsettings.Development.json`:
   ```json
   "ConnectionStrings": {
     "CatalogoConnection": "Server=localhost;Database=CatalogoDB;User Id=sa;Password=SuaSenhaForte;TrustServerCertificate=True;",
     "MongoConnection": "mongodb://localhost:27017"
   }
   ```

3. **Compile e Execute**
   ```bash
   dotnet restore
   dotnet run --project Catalogo.Api
   ```

4. **Acesse a Documentação**
   Abra: `http://localhost:5000/swagger`.

## Como Executar com Docker

1. **Build da Imagem**
   ```bash
   docker build -t burguer404-catalogo -f Catalogo.Api/Dockerfile .
   ```

2. **Rodar o Container**
   ```bash
   docker run -d -p 5000:8080 --name catalogo-api \
     -e ConnectionStrings__CatalogoConnection="Server=host.docker.internal;Database=CatalogoDB;User Id=sa;Password=SuaSenhaForte;TrustServerCertificate=True;" \
     -e ConnectionStrings__MongoConnection="mongodb://host.docker.internal:27017" \
     burguer404-catalogo
   ```

## Endpoints Principais

- `GET /api/produto/cardapio`: Retorna o cardápio completo organizado.
- `POST /api/produto`: Cria um novo produto.
- `GET /api/produto`: Lista produtos.
- `GET /api/produto/{id}`: Detalhes de um produto.

## Infraestrutura e Deploy

O projeto utiliza GitHub Actions para automação de testes, infraestrutura (Terraform) e deploy no Azure Kubernetes Service (AKS).

## SonarQube
- o	https://sonarcloud.io/project/overview?id=burguer404_Burguer404-Catalogo-MicroServico
