using AutoMapper;
using LocadoraNET.Application.Dtos;
using LocadoraNET.Domain;

namespace LocadoraNET.Application.Helpers
{
    public class LocadoraNetProfile: Profile
    {
        public LocadoraNetProfile()
        {
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<Filme, FilmeDto>();
            CreateMap<Locacao, LocacaoDto>();
        }
    }
}