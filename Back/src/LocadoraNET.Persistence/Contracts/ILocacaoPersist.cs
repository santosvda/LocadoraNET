using System.Threading.Tasks;
using LocadoraNET.Domain;

namespace LocadoraNET.Persistence.Contracts
{
    public interface ILocacaoPersist
    {
        Task<Locacao[]> GetAllLocacoes();
        Task<Locacao> GetLocacaoById(int LocacaoId);
        Task<Locacao[]> GetAllLocacoesByClienteId(int ClienteId);
        Task<Locacao[]> GetAllLocacoesByFilmeId(int FilmeId);
        string[] SqlRaw(string sql);
    }
}