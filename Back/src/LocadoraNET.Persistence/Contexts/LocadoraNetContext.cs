using LocadoraNET.Domain;
using LocadoraNET.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace LocadoraNET.Persistence.Contexts
{
    public class LocadoraNetContext : DbContext
    {
        public LocadoraNetContext(DbContextOptions<LocadoraNetContext> options) : base (options){ }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Locacao> Locacoes { get; set; }
        public DbSet<Filme> Filmes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            modelBuilder.ApplyConfiguration(new LocacaoConfiguration());
            modelBuilder.ApplyConfiguration(new FilmeConfiguration());
        }
    }
}