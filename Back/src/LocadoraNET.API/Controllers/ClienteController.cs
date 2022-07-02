using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LocadoraNET.Application.Contracts;
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
                if(clientes == null) return NotFound("No 'Clientes' found");

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
                if(cliente == null) return NotFound("No 'Clientes' found");

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error when trying to retrieve 'Clientes' Erro: {ex.Message}");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCliente(Cliente model)
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
                if(await _clienteService.DeleteCliente(clienteId)) return BadRequest("Failure to delete 'Cliente'");

                return Ok();
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error when trying to delete 'Clientes' Erro: {ex.Message}");
                throw;
            }
        }

        [HttpPut("{clienteId}")]
        public async Task<IActionResult> Get(int clienteId, Cliente model)
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
