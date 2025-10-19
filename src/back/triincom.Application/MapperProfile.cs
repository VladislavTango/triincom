using AutoMapper;
using triincom.Core.DTO;
using triincom.Core.Entities;

namespace triincom.Application
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<AddApplicationDto, ApplicationEntity>();
        }
    }
}
