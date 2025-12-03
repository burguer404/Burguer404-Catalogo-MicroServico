using Catalogo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Infrastructure.ContextDb
{
    public class CatalogoContext : DbContext
    {
        public CatalogoContext() { }

        public CatalogoContext(DbContextOptions<CatalogoContext> options) : base(options) { }

        public DbSet<ProdutoEntity> Produtos { get; set; }
        public DbSet<CategoriaEntity> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
