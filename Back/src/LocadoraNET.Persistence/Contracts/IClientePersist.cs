using System.Threading.Tasks;
using LocadoraNET.Domain;

namespace LocadoraNET.Persistence.Contracts
{
    public interface IClientePersist : IGeneralPersist
    {
        //Cliente
        Task<Cliente[]> GetAllClientes(bool includeLocacao = false);
        Task<Cliente> GetClienteById(int ClienteId, bool includeLocacao = false);
    }
}