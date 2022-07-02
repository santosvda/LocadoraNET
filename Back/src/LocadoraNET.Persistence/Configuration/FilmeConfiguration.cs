using LocadoraNET.Domain;
using Microsoft.EntityFrameworkCore;

namespace LocadoraNET.Persistence.Configuration
{
     public class FilmeConfiguration : IEntityTypeConfiguration<Filme>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Filme> builder)
        {
            builder.ToTable("Filme");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Titulo).HasMaxLength(100);
            builder.Property(p => p.ClassificacaoIndicativa).HasColumnType("TINYINT");

            builder.HasIndex(p => p.Lancamento).IsUnique(false);
            builder.HasIndex(p => p.Titulo).IsUnique(false);
        }
    }   

}