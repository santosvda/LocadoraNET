using LocadoraNET.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LocadoraNET.API.Configuration
{
     public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");
            builder.HasKey(p => p.ClienteId);

            builder.Property(p => p.Nome).HasMaxLength(200);
            builder.Property(p => p.Cpf).HasMaxLength(11);

            builder.HasIndex(p => p.Cpf).IsUnique(true);
            builder.HasIndex(p => p.Nome).IsUnique(false);
        }
    }   

}