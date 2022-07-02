using System.Linq;
using System.Threading.Tasks;
using LocadoraNET.Domain;
using LocadoraNET.Persistence.Contexts;
using LocadoraNET.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LocadoraNET.Persistence
{
    public class LocacaoPersist : ILocacaoPersist
    {
        private readonly LocadoraNetContext _context;
        public LocacaoPersist(LocadoraNetContext context)
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

        public async Task<Locacao[]> GetAllLocacoes()
        {
            IQueryable<Locacao> query = _context.Locacoes
            .Include(l => l.Cliente)
            .Include(l => l.Filme);

            query.OrderBy(c => c.Id);

            return await query.ToArrayAsync();
        }
        public async Task<Locacao[]> GetAllLocacoesByClienteId(int ClienteId)
        {
            IQueryable<Locacao> query = _context.Locacoes
            .Include(l => l.Cliente)
            .Include(l => l.Filme);

            query.OrderBy(c => c.Id).Where(c => c.ClienteId == ClienteId);

            return await query.ToArrayAsync();
        }
        public async Task<Locacao[]> GetAllLocacoesByFilmeId(int FilmeId)
        {
            IQueryable<Locacao> query = _context.Locacoes
            .Include(l => l.Cliente)
            .Include(l => l.Filme);

            query.OrderBy(c => c.Id).Where(c => c.FilmeId == FilmeId);

            return await query.ToArrayAsync();
        }
        public async Task<Locacao> GetLocacaoById(int LocacaoId)
        {
            IQueryable<Locacao> query = _context.Locacoes
            .Include(l => l.Cliente)
            .Include(l => l.Filme);

            query.OrderBy(c => c.Id).Where(c => c.Id == LocacaoId);

            return await query.FirstOrDefaultAsync();
        }
    }
}