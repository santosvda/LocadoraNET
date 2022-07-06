using System.Threading.Tasks;
using LocadoraNET.Application.Dtos;

namespace LocadoraNET.Application.Contracts
{
    public interface IFilmeService
    {
        Task<FilmeDto> AddFilme(FilmeDto model);
        Task<FilmeDto> UpdateFilme(int FilmeId, FilmeDto model);
        Task<bool> DeleteFilme(int FilmeId);

        Task<FilmeDto[]> GetAllFilmes(bool includeLocacao = false);
        Task<FilmeDto> GetFilmeById(int FilmeId, bool includeLocacao = false);
        Task<ImportDto> ImportCSV(ImportDto model);
    }
}