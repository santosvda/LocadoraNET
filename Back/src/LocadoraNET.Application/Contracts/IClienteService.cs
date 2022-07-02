using System.Threading.Tasks;
using LocadoraNET.Application.Dtos;

namespace LocadoraNET.Application.Contracts
{
    public interface IClienteService
    {
        Task<ClienteDto> AddCliente(ClienteDto model);
        Task<ClienteDto> UpdateCliente(int ClienteId, ClienteDto model);
        Task<bool> DeleteCliente(int ClienteId);

        Task<ClienteDto[]> GetAllClientes(bool includeLocacao = false);
        Task<ClienteDto> GetClienteById(int ClienteId, bool includeLocacao = false);
    }
}