using System;
using System.Threading.Tasks;
using LocadoraNET.Application.Contracts;
using LocadoraNET.Application.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraNET.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilmeController : ControllerBase
    {
        private readonly IFilmeService _filmeService;

        public FilmeController(IFilmeService filmeService)
        {
            _filmeService = filmeService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var filmes = await _filmeService.GetAllFilmes();
                if(filmes == null) return NoContent();

                return Ok(filmes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error when trying to retrieve 'Filmes' Erro: {ex.Message}");
                throw;
            }
        }

        [HttpGet("{filmeId}")]
        public async Task<IActionResult> GetById(int filmeId)
        {
            try
            {
                var filme = await _filmeService.GetFilmeById(filmeId);
                if(filme == null) return NoContent();

                return Ok(filme);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error when trying to retrieve 'Filmes' Erro: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFilme(FilmeDto model)
        {
            try
            {
                var filme = await _filmeService.AddFilme(model);
                if(filme == null) return BadRequest("Failure to save 'Filme'");

                return Ok(filme);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error when trying to save 'Filmes' Erro: {ex.Message}");
                throw;
            }
        }

        [HttpDelete("{filmeId}")]
        public async Task<IActionResult> DeleteFilme(int filmeId)
        {
            try
            {
                var filme = await _filmeService.GetFilmeById(filmeId);
                if(filme == null) return NoContent();

                return await _filmeService.DeleteFilme(filmeId) ? Ok("Deleted") : throw new Exception("Failure to delete 'Filme'");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error when trying to delete 'Filmes' Erro: {ex.Message}");
                throw;
            }
        }

        [HttpPut("{filmeId}")]
        public async Task<IActionResult> UpdateFilme(int filmeId, FilmeDto model)
        {
            try
            {
                var filme = await _filmeService.UpdateFilme(filmeId, model);
                if(filme == null) return BadRequest("Failure to update 'Filme'");

                return Ok(filme);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error when trying to update 'Filmes' Erro: {ex.Message}");
                throw;
            }
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportarCSV(ImportDto model)
        {
            try
            {
                var result = await _filmeService.ImportCSV(model);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                throw;
            }
        }
    }
}
