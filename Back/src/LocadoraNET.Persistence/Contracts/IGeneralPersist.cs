using System.Threading.Tasks;
using LocadoraNET.Domain;

namespace LocadoraNET.Persistence.Contracts
{
    public interface IGeneralPersist
    {
        void Add<T>(T entity) where T: class;
        void Update<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        void DeleteRange<T>(T[] entityArray) where T: class;
        Task<bool> SaveChangesAsync<T>(T entity) where T: class;
    }
}