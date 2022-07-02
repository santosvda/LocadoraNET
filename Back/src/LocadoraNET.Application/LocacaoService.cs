using System;
using System.Threading.Tasks;
using AutoMapper;
using LocadoraNET.Application.Contracts;
using LocadoraNET.Application.Dtos;
using LocadoraNET.Domain;
using LocadoraNET.Persistence.Contracts;

namespace LocadoraNET.Application
{
    public class LocacaoService : ILocacaoService
    {
        private readonly IGeneralPersist _generalPersist;
        private readonly ILocacaoPersist _locacaoPersist;
        private readonly IMapper _mapper;

        public LocacaoService(IGeneralPersist generalPersist, ILocacaoPersist locacaoPersist, IMapper mapper)
        {
            _generalPersist = generalPersist;
            _locacaoPersist = locacaoPersist;
            _mapper = mapper;
        }
        public async Task<LocacaoDto> AddLocacao(LocacaoDto model)
        {
            try
            {
                var locacao = _mapper.Map<Locacao>(model);
                _generalPersist.Add<Locacao>(locacao);

                if(!await _generalPersist.SaveChangesAsync())
                    return null;
                    
                var result = await _locacaoPersist.GetLocacaoById(locacao.Id);
                return _mapper.Map<LocacaoDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LocacaoDto> UpdateLocacao(int locacaoId, LocacaoDto model)
        {
            try
            {
                var locacao = await _locacaoPersist.GetLocacaoById(locacaoId);
                if(locacao == null) return null;

                model.Id = locacao.Id;
                _mapper.Map(model, locacao);
                _generalPersist.Update<Locacao>(locacao);

                if(!await _generalPersist.SaveChangesAsync())
                    return null;
                    
                var result = await _locacaoPersist.GetLocacaoById(locacao.Id);
                return _mapper.Map<LocacaoDto>(result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteLocacao(int locacaoId)
        {
            try
            {
                var locacao = await _locacaoPersist.GetLocacaoById(locacaoId);
                
                if(locacao == null) throw new Exception("'Locacao' for delete not found!");

                _generalPersist.Delete<Locacao>(locacao);

                return await _generalPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LocacaoDto[]> GetAllLocacoes()
        {
            try
            {
                var locacao = await _locacaoPersist.GetAllLocacoes();
                if(locacao == null) return null;

                return _mapper.Map<LocacaoDto[]>(locacao);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LocacaoDto> GetLocacaoById(int locacaoId)
        {
            try
            {
                var locacao = await _locacaoPersist.GetLocacaoById(locacaoId);
                if(locacao == null) return null;

                return _mapper.Map<LocacaoDto>(locacao);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LocacaoDto[]> GetAllLocacoesByClienteId(int clienteId)
        {
            try
            {
                var locacao = await _locacaoPersist.GetAllLocacoesByClienteId(clienteId);
                if(locacao == null) return null;

                return _mapper.Map<LocacaoDto[]>(locacao);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LocacaoDto[]> GetAllLocacoesByFilmeId(int filmeId)
        {
            try
            {
                var locacao = await _locacaoPersist.GetAllLocacoesByFilmeId(filmeId);
                if(locacao == null) return null;

                return _mapper.Map<LocacaoDto[]>(locacao);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}