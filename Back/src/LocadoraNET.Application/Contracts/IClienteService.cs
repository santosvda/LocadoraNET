using System.Threading.Tasks;
using LocadoraNET.Domain;

namespace LocadoraNET.Application.Contracts
{
    public interface IClienteService
    {
        Task<Cliente> AddCliente(Cliente model);
        Task<Cliente> UpdateCliente(int ClienteId, Cliente model);
        Task<bool> DeleteCliente(int ClienteId);

        Task<Cliente[]> GetAllClientes(bool includeLocacao = false);
        Task<Cliente> GetClienteById(int ClienteId, bool includeLocacao = false);
    }
}