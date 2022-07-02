using System;
using System.Linq;
using System.Threading.Tasks;
using LocadoraNET.Domain;
using LocadoraNET.Persistence.Contexts;
using LocadoraNET.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LocadoraNET.Persistence
{
    public class ClientePersist
    {
        private readonly LocadoraNetContext _context;
        public ClientePersist(LocadoraNetContext context)
        {
            _context = context;
        }

        public async Task<Cliente[]> GetAllClientes(bool includeLocacao = false)
        {
            IQueryable<Cliente> query = _context.Clientes;

            if(includeLocacao) 
                query = query
                        .Include(c => c.Locacoes)
                        .ThenInclude(l => l.Filme);

            query.OrderBy(c => c.Id);

            return await query.ToArrayAsync();
        }
        public async Task<Cliente> GetClienteById(int ClienteId, bool includeLocacao = false)
        {
            IQueryable<Cliente> query = _context.Clientes;

            if(includeLocacao) 
                query = query
                        .Include(c => c.Locacoes)
                        .ThenInclude(l => l.Filme);

            query.OrderBy(c => c.Id).Where(c => c.Id == ClienteId);

            return await query.FirstOrDefaultAsync();
        }
    }
}