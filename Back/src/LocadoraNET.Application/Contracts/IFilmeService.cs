using System.Threading.Tasks;
using LocadoraNET.Domain;

namespace LocadoraNET.Application.Contracts
{
    public interface IFilmeService
    {
        Task<Filme> AddFilme(Filme model);
        Task<Filme> UpdateFilme(int FilmeId, Filme model);
        Task<bool> DeleteFilme(int FilmeId);

        Task<Filme[]> GetAllFilmes(bool includeLocacao = false);
        Task<Filme> GetFilmeById(int FilmeId, bool includeLocacao = false);
    }
}