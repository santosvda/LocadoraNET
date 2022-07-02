using System;
using System.Threading.Tasks;
using LocadoraNET.Application.Contracts;
using LocadoraNET.Domain;
using LocadoraNET.Persistence.Contracts;

namespace LocadoraNET.Application
{
    public class FilmeService : IFilmeService
    {
        private readonly IGeneralPersist _generalPersist;
        private readonly IFilmePersist _filmePersist;

        public FilmeService(IGeneralPersist generalPersist, IFilmePersist filmePersist)
        {
            _generalPersist = generalPersist;
            _filmePersist = filmePersist;
        }
        public async Task<Filme> AddFilme(Filme model)
        {
            try
            {
                _generalPersist.Add<Filme>(model);

                if(await _generalPersist.SaveChangesAsync())
                    return model;

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Filme> UpdateFilme(int FilmeId, Filme model)
        {
            try
            {
                var filme = await _filmePersist.GetFilmeById(FilmeId);
                
                if(filme == null) return null;

                model.Id = filme.Id;

                _generalPersist.Update(model);

                if(await _generalPersist.SaveChangesAsync())
                    return await _filmePersist.GetFilmeById(model.Id);

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteFilme(int FilmeId)
        {
            try
            {
                var filme = await _filmePersist.GetFilmeById(FilmeId);
                
                if(filme == null) throw new Exception("'Filme' for delete not found!");

                _generalPersist.Delete<Filme>(filme);

                return await _generalPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Filme[]> GetAllFilmes(bool includeLocacao = false)
        {
            try
            {
                var filmes = await _filmePersist.GetAllFilmes();
                if(filmes == null) return null;

                return filmes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Filme> GetFilmeById(int FilmeId, bool includeLocacao = false)
        {
            try
            {
                var filme = await _filmePersist.GetFilmeById(FilmeId);
                if(filme == null) return null;

                return filme;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}