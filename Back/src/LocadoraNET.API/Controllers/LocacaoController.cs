using System;
using System.Threading.Tasks;
using LocadoraNET.Application.Contracts;
using LocadoraNET.Application.Dtos;
using LocadoraNET.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraNET.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocacaoController : ControllerBase
    {
        private readonly ILocacaoService _locacaoService;

        public LocacaoController(ILocacaoService locacaoService)
        {
            _locacaoService = locacaoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var locacoes = await _locacaoService.GetAllLocacoes();
                if(locacoes == null) return NoContent();

                return Ok(locacoes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error when trying to retrieve 'Locacao' Erro: {ex.Message}");
                throw;
            }
        }

        [HttpGet("{locacaoId}")]
        public async Task<IActionResult> GetById(int locacaoId)
        {
            try
            {
                var locacao = await _locacaoService.GetLocacaoById(locacaoId);
                if(locacao == null) return NoContent();

                return Ok(locacao);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error when trying to retrieve 'Locacao' Erro: {ex.Message}");
                throw;
            }
        }

        [HttpGet("cliente/{clienteId}")]
        public async Task<IActionResult> GetByClienteId(int clienteId)
        {
            try
            {
                var locacoes = await _locacaoService.GetAllLocacoesByClienteId(clienteId);
                if(locacoes == null) return NoContent();

                return Ok(locacoes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error when trying to retrieve 'Locacao' Erro: {ex.Message}");
                throw;
            }
        }

        [HttpGet("filme/{filmeId}")]
        public async Task<IActionResult> GetByFilmeId(int filmeId)
        {
            try
            {
                var locacoes = await _locacaoService.GetAllLocacoesByFilmeId(filmeId);
                if(locacoes == null) return NoContent();

                return Ok(locacoes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error when trying to retrieve 'Locacao' Erro: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddLocacao(LocacaoDto model)
        {
            try
            {
                var locacao = await _locacaoService.AddLocacao(model);
                if(locacao == null) return BadRequest("Failure to save 'Locacao'");

                return Ok(locacao);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error when trying to save 'Locacao' Erro: {ex.Message}");
                throw;
            }
        }

        [HttpDelete("{locacaoId}")]
        public async Task<IActionResult> DeleteLocacao(int locacaoId)
        {
            try
            {
                var locacao = await _locacaoService.GetLocacaoById(locacaoId);
                if(locacao == null) return NoContent();

                return await _locacaoService.DeleteLocacao(locacaoId) ? Ok("Deleted") : throw new Exception("Failure to delete 'Locacao'");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error when trying to delete 'Locacao' Erro: {ex.Message}");
                throw;
            }
        }

        [HttpPut("{locacaoId}")]
        public async Task<IActionResult> UpdateLocacao(int locacaoId, LocacaoDto model)
        {
            try
            {
                var locacao = await _locacaoService.UpdateLocacao(locacaoId, model);
                if(locacao == null) return BadRequest("Failure to update 'Locacao'");

                return Ok(locacao);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error when trying to update 'Locacao' Erro: {ex.Message}");
                throw;
            }
        }
        
        [HttpGet("gerar/planilha")]
        public FileResult Planilha()
        {
            try
            {
                const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Response.ContentType = contentType;
                HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

                var fileContentResult = new FileContentResult(_locacaoService.GerarPlanilha(), contentType)
                {
                    FileDownloadName = "File.xlsx"
                };

                return fileContentResult;
            }
            catch
            {
                return null;
                throw;
            }
        }
    }
}
