using System;
using System.Threading.Tasks;
using AutoMapper;
using LocadoraNET.Application.Contracts;
using LocadoraNET.Application.Dtos;
using LocadoraNET.Domain;
using LocadoraNET.Persistence.Contracts;

namespace LocadoraNET.Application
{
    public class FilmeService : IFilmeService
    {
        private readonly IGeneralPersist _generalPersist;
        private readonly IFilmePersist _FilmePersist;
        private readonly IMapper _mapper;

        public FilmeService(IGeneralPersist generalPersist, IFilmePersist FilmePersist, IMapper mapper)
        {
            _generalPersist = generalPersist;
            _FilmePersist = FilmePersist;
            _mapper = mapper;
        }
        public async Task<FilmeDto> AddFilme(FilmeDto model)
        {
            try
            {
                var Filme = _mapper.Map<Filme>(model);
                _generalPersist.Add<Filme>(Filme);

                if(!await _generalPersist.SaveChangesAsync())
                    return null;
                    
                var result = await _FilmePersist.GetFilmeById(Filme.Id);
                return _mapper.Map<FilmeDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<FilmeDto> UpdateFilme(int FilmeId, FilmeDto model)
        {
            try
            {
                var Filme = await _FilmePersist.GetFilmeById(FilmeId);
                if(Filme == null) return null;

                model.Id = Filme.Id;
                _mapper.Map(model, Filme);
                _generalPersist.Update<Filme>(Filme);

                if(!await _generalPersist.SaveChangesAsync())
                    return null;
                    
                var result = await _FilmePersist.GetFilmeById(Filme.Id);
                return _mapper.Map<FilmeDto>(result);

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
                var Filme = await _FilmePersist.GetFilmeById(FilmeId);
                
                if(Filme == null) throw new Exception("'Filme' for delete not found!");

                _generalPersist.Delete<Filme>(Filme);

                return await _generalPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<FilmeDto[]> GetAllFilmes(bool includeLocacao = false)
        {
            try
            {
                var Filmes = await _FilmePersist.GetAllFilmes();
                if(Filmes == null) return null;

                return _mapper.Map<FilmeDto[]>(Filmes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<FilmeDto> GetFilmeById(int FilmeId, bool includeLocacao = false)
        {
            try
            {
                var Filme = await _FilmePersist.GetFilmeById(FilmeId);
                if(Filme == null) return null;

                return _mapper.Map<FilmeDto>(Filme);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}