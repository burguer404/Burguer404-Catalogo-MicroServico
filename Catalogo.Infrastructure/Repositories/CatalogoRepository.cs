using Catalogo.Domain.Entities;
using Catalogo.Domain.Arguments;
using Catalogo.Infrastructure.ContextDb;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Infrastructure.Repositories
{
    public class CatalogoRepository
    {
        private readonly CatalogoContext _context;

        public CatalogoRepository(CatalogoContext context)
        {
            _context = context;
        }

        public async Task<ProdutoEntity?> CriarProduto(ProdutoEntity produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return produto;
        }

        public async Task<List<ProdutoEntity>?> ListarProdutos()
        {
            return await _context.Produtos
                .Include(p => p.Categoria)
                .Where(p => p.Status)
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }

        public async Task<ProdutoEntity?> ObterProdutoPorId(int id)
        {
            return await _context.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ProdutoEntity?> AtualizarProduto(ProdutoEntity produto)
        {
            var produtoExistente = await _context.Produtos.FindAsync(produto.Id);
            if (produtoExistente == null)
                return null;

            produtoExistente.Nome = produto.Nome;
            produtoExistente.Descricao = produto.Descricao;
            produtoExistente.Preco = produto.Preco;
            produtoExistente.CategoriaId = produto.CategoriaId;
            produtoExistente.Status = produto.Status;
            produtoExistente.DataAtualizacao = DateTime.Now;

            _context.Produtos.Update(produtoExistente);
            await _context.SaveChangesAsync();
            return produtoExistente;
        }

        public async Task<bool> RemoverProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
                return false;

            produto.Status = false;
            produto.DataAtualizacao = DateTime.Now;
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProdutoEntity>?> ObterProdutosPorCategoria(int categoriaId)
        {
            return await _context.Produtos
                .Include(p => p.Categoria)
                .Where(p => p.CategoriaId == categoriaId && p.Status)
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }

        public async Task<CardapioResponse> ObterCardapio()
        {
            var produtos = await _context.Produtos
                .Include(p => p.Categoria)
                .Where(p => p.Status)
                .OrderBy(p => p.CategoriaId)
                .ThenBy(p => p.Nome)
                .ToListAsync();

            var categorias = await _context.Categorias
                .Where(c => c.Ativa)
                .OrderBy(c => c.Id)
                .ToListAsync();

            var cardapio = new CardapioResponse
            {
                Sucesso = true,
                Mensagem = "Cardápio obtido com sucesso",
                TotalProdutos = produtos.Count,
                DataAtualizacao = DateTime.Now
            };

            foreach (var categoria in categorias)
            {
                var produtosDaCategoria = produtos
                    .Where(p => p.CategoriaId == categoria.Id)
                    .Select(p => new ProdutoResponse
                    {
                        Id = p.Id,
                        Nome = p.Nome,
                        Descricao = p.Descricao,
                        Preco = p.Preco,
                        CategoriaId = p.CategoriaId,
                        CategoriaDescricao = p.Categoria?.Descricao ?? string.Empty,
                        ImagemBase64 = string.Empty,
                        Status = p.Status,
                        DataCriacao = p.DataCriacao,
                        DataAtualizacao = p.DataAtualizacao
                    })
                    .ToList();

                var categoriaResponse = new CategoriaResponse
                {
                    Id = categoria.Id,
                    Descricao = categoria.Descricao,
                    Ativa = categoria.Ativa,
                    Produtos = produtosDaCategoria
                };

                cardapio.Categorias.Add(categoriaResponse);
            }

            return cardapio;
        }

        public async Task<List<CategoriaEntity>?> ListarCategorias()
        {
            return await _context.Categorias
                .Where(c => c.Ativa)
                .OrderBy(c => c.Descricao)
                .ToListAsync();
        }

        public async Task<CategoriaEntity?> CriarCategoria(CategoriaEntity categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }
    }
}
