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
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var clientes = await _clienteService.GetAllClientes();
                if(clientes == null) return NoContent();

                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error when trying to retrieve 'Clientes' Erro: {ex.Message}");
                throw;
            }
        }

        [HttpGet("{clienteId}")]
        public async Task<IActionResult> GetById(int clienteId)
        {
            try
            {
                var cliente = await _clienteService.GetClienteById(clienteId);
                if(cliente == null) return NoContent();

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error when trying to retrieve 'Clientes' Erro: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCliente(ClienteDto model)
        {
            try
            {
                var cliente = await _clienteService.AddCliente(model);
                if(cliente == null) return BadRequest("Failure to save 'Cliente'");

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error when trying to save 'Clientes' Erro: {ex.Message}");
                throw;
            }
        }

        [HttpDelete("{clienteId}")]
        public async Task<IActionResult> DeleteCliente(int clienteId)
        {
            try
            {
                var cliente = await _clienteService.GetClienteById(clienteId);
                if(cliente == null) return NoContent();

                return await _clienteService.DeleteCliente(clienteId) ? Ok("Deleted") : throw new Exception("Failure to delete 'Cliente'");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error when trying to delete 'Clientes' Erro: {ex.Message}");
                throw;
            }
        }

        [HttpPut("{clienteId}")]
        public async Task<IActionResult> UpdateCliente(int clienteId, ClienteDto model)
        {
            try
            {
                var cliente = await _clienteService.UpdateCliente(clienteId, model);
                if(cliente == null) return BadRequest("Failure to update 'Cliente'");

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error when trying to update 'Clientes' Erro: {ex.Message}");
                throw;
            }
        }
    }
}
