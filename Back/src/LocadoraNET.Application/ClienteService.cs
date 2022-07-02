using System;
using System.Threading.Tasks;
using LocadoraNET.Application.Contracts;
using LocadoraNET.Domain;
using LocadoraNET.Persistence.Contracts;

namespace LocadoraNET.Application
{
    public class ClienteService : IClienteService
    {
        private readonly IGeneralPersist _generalPersist;
        private readonly IClientePersist _clientePersist;

        public ClienteService(IGeneralPersist generalPersist, IClientePersist clientePersist)
        {
            _generalPersist = generalPersist;
            _clientePersist = clientePersist;
        }
        public async Task<Cliente> AddCliente(Cliente model)
        {
            try
            {
                _generalPersist.Add<Cliente>(model);

                if(await _generalPersist.SaveChangesAsync())
                    return await _clientePersist.GetClienteById(model.Id);

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Cliente> UpdateCliente(int clienteId, Cliente model)
        {
            try
            {
                var cliente = await _clientePersist.GetClienteById(clienteId);
                
                if(cliente == null) return null;

                model.Id = cliente.Id;

                _generalPersist.Update(model);

                if(await _generalPersist.SaveChangesAsync())
                    return await _clientePersist.GetClienteById(model.Id);

                return null;
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

        public async Task<Cliente[]> GetAllClientes(bool includeLocacao = false)
        {
            try
            {
                var clientes = await _clientePersist.GetAllClientes();
                if(clientes == null) return null;

                return clientes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Cliente> GetClienteById(int clienteId, bool includeLocacao = false)
        {
            try
            {
                var cliente = await _clientePersist.GetClienteById(clienteId);
                if(cliente == null) return null;

                return cliente;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}