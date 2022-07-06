using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using LocadoraNET.Application.Contracts;
using LocadoraNET.Application.Dtos;
using LocadoraNET.Domain;
using LocadoraNET.Persistence.Contracts;
using Microsoft.VisualBasic.FileIO;

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

        public async Task<ImportDto> ImportCSV(ImportDto model)
        {
            try
            {
                Regex regex=new Regex(@"^[\w/\:.-]+;base64,");
                string base64Str = regex.Replace(model.File64, string.Empty);
                byte[] fileBytes = Convert.FromBase64String(base64Str);
                string strFileData = System.Text.Encoding.UTF8.GetString(fileBytes, 0, fileBytes.Length);
                Stream stream = new MemoryStream(fileBytes);

                var result = new ImportDto() { KeyError = false };

                using (TextFieldParser parser = new TextFieldParser(stream))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(";");
                    bool firstRow = true;
                    bool isAdd = false;
                    while (!parser.EndOfData)
                    {
                        string[] row = parser.ReadFields();
                        if (firstRow)
                        {
                            firstRow = false;
                            continue;
                        }

                        //Processing row
                        var filmeCsv = new FilmeDto() { 
                            Id = Convert.ToInt32(row[0]),
                            Titulo = row[1],
                            ClassificacaoIndicativa = Convert.ToInt32(row[2]),
                            Lancamento = Convert.ToInt32(row[3])
                        };

                        var validate = await _FilmePersist.GetFilmeById(filmeCsv.Id);
                        if (validate != null)
                        {
                            result.KeyError = true;
                            continue;
                        }

                        var filme = _mapper.Map<Filme>(filmeCsv);
                        _generalPersist.Add<Filme>(filme);
                        isAdd = true;
                    }
                    if (!isAdd)
                        result.AddError = true;
                    else if (!await _generalPersist.SaveChangesAsync())
                       throw new Exception("Falha ao salvar");
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}