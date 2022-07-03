using System.Threading.Tasks;
using LocadoraNET.Application.Dtos;

namespace LocadoraNET.Application.Contracts
{
    public interface ILocacaoService
    {
        Task<LocacaoDto> AddLocacao(LocacaoDto model);
        Task<LocacaoDto> UpdateLocacao(int locacaoId, LocacaoDto model);
        Task<bool> DeleteLocacao(int locacaoId);

        Task<LocacaoDto[]> GetAllLocacoes();
        Task<LocacaoDto> GetLocacaoById(int locacaoId);
        Task<LocacaoDto[]> GetAllLocacoesByClienteId(int ClienteId);
        Task<LocacaoDto[]> GetAllLocacoesByFilmeId(int filmeId);
    }
}