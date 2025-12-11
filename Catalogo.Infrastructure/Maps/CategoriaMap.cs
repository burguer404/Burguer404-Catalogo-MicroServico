using Catalogo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalogo.Infrastructure.Maps
{
    public class CategoriaMap : IEntityTypeConfiguration<CategoriaEntity>
    {
        public void Configure(EntityTypeBuilder<CategoriaEntity> builder)
        {
            builder.ToTable("Categorias");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.Descricao).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Ativa).IsRequired();

            builder.HasIndex(x => x.Descricao).IsUnique();

            builder.HasData(
                new CategoriaEntity { Id = 1, Descricao = "Lanche" },
                new CategoriaEntity { Id = 2, Descricao = "Acompanhamento" },
                new CategoriaEntity { Id = 3, Descricao = "Bebida" },
                new CategoriaEntity { Id = 4, Descricao = "Sobremesa" }
            );
        }
    }
}
