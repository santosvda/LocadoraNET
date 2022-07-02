using System.Linq;
using System.Threading.Tasks;
using LocadoraNET.Domain;
using LocadoraNET.Persistence.Contexts;
using LocadoraNET.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LocadoraNET.Persistence
{
    public class FilmePersist : IFilmePersist
    {
        private readonly LocadoraNetContext _context;
        public FilmePersist(LocadoraNetContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public void DeleteRange<T>(T[] entityArray) where T : class
        {
            _context.RemoveRange(entityArray);
        }
        public async Task<bool> SaveChangesAsync<T>(T entity) where T : class
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
        
        public async Task<Filme[]> GetAllFilmes(bool includeLocacao = false)
        {
            IQueryable<Filme> query = _context.Filmes;

            if(includeLocacao) 
                query = query
                        .Include(f => f.Locacoes)
                        .ThenInclude(c => c.Cliente);

            query.OrderBy(f => f.Id);

            return await query.ToArrayAsync();
        }
        public async Task<Filme> GetFilmeById(int FilmeId, bool includeLocacao = false)
        {
            IQueryable<Filme> query = _context.Filmes;

            if(includeLocacao) 
            query = query
                    .Include(f => f.Locacoes)
                    .ThenInclude(c => c.Cliente);

            query.OrderBy(f => f.Id).Where(f => f.Id == FilmeId);

            return await query.FirstOrDefaultAsync();
        }
    }
}