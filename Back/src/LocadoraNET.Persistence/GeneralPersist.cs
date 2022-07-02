using System.Threading.Tasks;
using LocadoraNET.Persistence.Contexts;
using LocadoraNET.Persistence.Contracts;

namespace LocadoraNET.Persistence
{
    public class GeneralPersist : IGeneralPersist
    {
        private readonly LocadoraNetContext _context;

        public GeneralPersist(LocadoraNetContext context)
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
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}