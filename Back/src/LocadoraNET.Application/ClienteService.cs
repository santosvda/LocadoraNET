using System;
using System.Threading.Tasks;
using AutoMapper;
using LocadoraNET.Application.Contracts;
using LocadoraNET.Application.Dtos;
using LocadoraNET.Application.Helpers;
using LocadoraNET.Domain;
using LocadoraNET.Persistence.Contracts;

namespace LocadoraNET.Application
{
    public class ClienteService : IClienteService
    {
        private readonly IGeneralPersist _generalPersist;
        private readonly IClientePersist _clientePersist;
        private readonly IMapper _mapper;

        public ClienteService(IGeneralPersist generalPersist, IClientePersist clientePersist, IMapper mapper)
        {
            _generalPersist = generalPersist;
            _clientePersist = clientePersist;
            _mapper = mapper;
        }
        public async Task<ClienteDto> AddCliente(ClienteDto model)
        {
            try
            {
                var data = Utils.StringToDate(model.DataNascimento);

                var cliente = _mapper.Map<Cliente>(model);
                cliente.DataNascimento = data;
                _generalPersist.Add<Cliente>(cliente);

                if(!await _generalPersist.SaveChangesAsync())
                    return null;
                    
                var result = await _clientePersist.GetClienteById(cliente.Id);
                return _mapper.Map<ClienteDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ClienteDto> UpdateCliente(int clienteId, ClienteDto model)
        {
            try
            {
                var cliente = await _clientePersist.GetClienteById(clienteId);
                if(cliente == null) return null;

                var data = Utils.StringToDate(model.DataNascimento);
                model.Id = cliente.Id;
                _mapper.Map(model, cliente);
                cliente.DataNascimento = data;
                _generalPersist.Update<Cliente>(cliente);

                if(!await _generalPersist.SaveChangesAsync())
                    return null;
                    
                var result = await _clientePersist.GetClienteById(cliente.Id);
                return _mapper.Map<ClienteDto>(result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteCliente(int clienteId)
        {
            try
            {
                var cliente = await _clientePersist.GetClienteById(clienteId);
                
                if(cliente == null) throw new Exception("'Cliente' for delete not found!");

                _generalPersist.Delete<Cliente>(cliente);

                return await _generalPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ClienteDto[]> GetAllClientes(bool includeLocacao = false)
        {
            try
            {
                var clientes = await _clientePersist.GetAllClientes();
                if(clientes == null) return null;

                return _mapper.Map<ClienteDto[]>(clientes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ClienteDto> GetClienteById(int clienteId, bool includeLocacao = false)
        {
            try
            {
                var cliente = await _clientePersist.GetClienteById(clienteId);
                if(cliente == null) return null;

                return _mapper.Map<ClienteDto>(cliente);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}