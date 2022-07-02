using LocadoraNET.Domain;
using Microsoft.EntityFrameworkCore;

namespace LocadoraNET.Persistence.Configuration
{
     public class LocacaoConfiguration : IEntityTypeConfiguration<Locacao>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Locacao> builder)
        {
            builder.ToTable("Locacao");
            builder.HasKey(p => p.Id);
        }
    }   

}