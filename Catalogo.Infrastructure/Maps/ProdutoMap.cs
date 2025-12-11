using Catalogo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalogo.Infrastructure.Maps
{
    public class ProdutoMap : IEntityTypeConfiguration<ProdutoEntity>
    {
        public void Configure(EntityTypeBuilder<ProdutoEntity> builder)
        {
            builder.ToTable("Produtos");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.Nome).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Descricao).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Preco).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.CategoriaId).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.DataCriacao).IsRequired();
            builder.Property(x => x.DataAtualizacao);

            builder.HasOne(x => x.Categoria)
                .WithMany()
                .HasForeignKey(x => x.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Nome);
            builder.HasIndex(x => x.CategoriaId);
            builder.HasIndex(x => x.Status);

            builder.HasData(
                new ProdutoEntity { Id = 1, Nome = "X-Bacon", Descricao = "adicional de bacon", Preco = 31.99, CategoriaId = 1 },
                new ProdutoEntity { Id = 2, Nome = "Coca-Cola", Descricao = "Zero açucar", Preco = 7.0, CategoriaId = 3 },
                new ProdutoEntity { Id = 3, Nome = "Batata frita", Descricao = "300g", Preco = 15.0, CategoriaId = 2 },
                new ProdutoEntity { Id = 4, Nome = "Sorvete", Descricao = "Morango", Preco = 9.0, CategoriaId = 4 },
                new ProdutoEntity { Id = 5, Nome = "X-Salada", Descricao = "saladinha da boa", Preco = 24.99, CategoriaId = 1 },
                new ProdutoEntity { Id = 6, Nome = "Pepsi", Descricao = "concorrente", Preco = 7.0, CategoriaId = 3 },
                new ProdutoEntity { Id = 7, Nome = "Onion rings", Descricao = "300g", Preco = 20.0, CategoriaId = 2 },
                new ProdutoEntity { Id = 8, Nome = "Bolo de pote", Descricao = "Chocolate com morango", Preco = 14.0, CategoriaId = 4 },
                new ProdutoEntity { Id = 9, Nome = "X-Tudo", Descricao = "tudo do bom e do melhor", Preco = 40.0, CategoriaId = 1 },
                new ProdutoEntity { Id = 10, Nome = "Suco de maracuja", Descricao = "suquinho", Preco = 10.0, CategoriaId = 3 },
                new ProdutoEntity { Id = 11, Nome = "Batata + Onion rings P", Descricao = "400g", Preco = 27.5, CategoriaId = 2 },
                new ProdutoEntity { Id = 12, Nome = "Pudim", Descricao = "Melhor de todos", Preco = 99.0, CategoriaId = 4 },
                new ProdutoEntity { Id = 13, Nome = "X-Frango", Descricao = "fitness", Preco = 22.99, CategoriaId = 1 },
                new ProdutoEntity { Id = 14, Nome = "X-Calabresa", Descricao = "pouca gordura graças a Deus", Preco = 26.99, CategoriaId = 1 },
                new ProdutoEntity { Id = 15, Nome = "X-Picanha", Descricao = "suculência ao máximo", Preco = 36.99, CategoriaId = 1 },
                new ProdutoEntity { Id = 16, Nome = "Suco de limão", Descricao = "suquinho 2", Preco = 7.0, CategoriaId = 3 },
                new ProdutoEntity { Id = 17, Nome = "H2O", Descricao = "água de torneira", Preco = 5.0, CategoriaId = 3 },
                new ProdutoEntity { Id = 18, Nome = "Batata + Onion rings M", Descricao = "700g", Preco = 33.0, CategoriaId = 2 },
                new ProdutoEntity { Id = 19, Nome = "Batata + Onion rings G", Descricao = "1Kg", Preco = 41.0, CategoriaId = 2 }
            );
        }
    }
}
