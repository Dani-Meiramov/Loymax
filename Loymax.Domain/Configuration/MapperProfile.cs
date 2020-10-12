using AutoMapper;
using Loymax.Domain.Data.ModelsDto;
using Loymax.Domain.Data.Models;

namespace Loymax.Domain.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateClientDto, Client>();
            CreateMap<Client, ClientDto>();
        }
    }
}