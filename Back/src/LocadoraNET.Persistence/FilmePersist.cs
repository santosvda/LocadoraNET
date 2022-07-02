using System.Linq;
using System.Threading.Tasks;
using LocadoraNET.Domain;
using LocadoraNET.Persistence.Contexts;
using LocadoraNET.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LocadoraNET.Persistence
{
    public class FilmePersist
    {
        private readonly LocadoraNetContext _context;
        public FilmePersist(LocadoraNetContext context)
        {
            _context = context;
        }
        
        public async Task<Filme[]> GetAllFilmes(bool includeLocacao = false)
        {
            IQueryable<Filme> query = _context.Filmes;

            if(includeLocacao) 
                query = query
                        .Include(f => f.Locacoes)
                        .ThenInclude(c => c.Cliente);

            query = query.AsNoTracking().OrderBy(f => f.Id);

            return await query.ToArrayAsync();
        }
        public async Task<Filme> GetFilmeById(int FilmeId, bool includeLocacao = false)
        {
            IQueryable<Filme> query = _context.Filmes;

            if(includeLocacao) 
            query = query
                    .Include(f => f.Locacoes)
                    .ThenInclude(c => c.Cliente);

            query = query.AsNoTracking().OrderBy(f => f.Id).Where(f => f.Id == FilmeId);

            return await query.FirstOrDefaultAsync();
        }
    }
}