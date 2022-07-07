using System.Collections.Generic;
using System.Data;
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
        
        public async Task<Locacao[]> GetAllLocacoes()
        {
            IQueryable<Locacao> query = _context.Locacoes
            .Include(l => l.Cliente)
            .Include(l => l.Filme);

            query = query.AsNoTracking().OrderBy(c => c.Id);

            return await query.ToArrayAsync();
        }
        public async Task<Locacao[]> GetAllLocacoesByClienteId(int ClienteId)
        {
            IQueryable<Locacao> query = _context.Locacoes
            .Include(l => l.Cliente)
            .Include(l => l.Filme);

            query = query.AsNoTracking().OrderBy(c => c.Id).Where(c => c.Id == ClienteId);

            return await query.ToArrayAsync();
        }
        public async Task<Locacao[]> GetAllLocacoesByFilmeId(int FilmeId)
        {
            IQueryable<Locacao> query = _context.Locacoes
            .Include(l => l.Cliente)
            .Include(l => l.Filme);

            query = query.AsNoTracking().OrderBy(c => c.Id).Where(c => c.FilmeId == FilmeId);

            return await query.ToArrayAsync();
        }
        public async Task<Locacao> GetLocacaoById(int LocacaoId)
        {
            IQueryable<Locacao> query = _context.Locacoes
            .Include(l => l.Cliente)
            .Include(l => l.Filme);

            query = query.AsNoTracking().OrderBy(c => c.Id).Where(c => c.Id == LocacaoId);

            return await query.FirstOrDefaultAsync();
        }

        public string[] SqlRaw(string sql)
        {
            var aux = new List<string>();
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                _context.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        aux.Add(result[0].ToString());
                    }
                }
            }
            return aux.ToArray();
        }
    }
}