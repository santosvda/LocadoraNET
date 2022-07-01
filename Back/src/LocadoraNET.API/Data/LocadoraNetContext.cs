using System.Linq;
using LocadoraNET.API.Configuration;
using LocadoraNET.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LocadoraNET.Data
{
    public class LocadoraNetContext : DbContext
    {
        public LocadoraNetContext(DbContextOptions<LocadoraNetContext> options) : base (options){ }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
        }
    }
}