using System.Threading.Tasks;
using LocadoraNET.Domain;

namespace LocadoraNET.Persistence.Contracts
{
    public interface IFilmePersist : IGeneralPersist
    {
        //Filme
        Task<Filme[]> GetAllFilmes(bool includeLocacao = false);
        Task<Filme> GetFilmeById(int FilmeId, bool includeLocacao = false);
    }
}