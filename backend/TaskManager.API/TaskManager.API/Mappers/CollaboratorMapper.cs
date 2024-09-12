using AutoMapper;
using TaskManager.API.DTO.Response;
using TaskManager.Domain.Entities;

namespace TaskManager.API.Mappers
{
    public class CollaboratorMapper : Profile
    {
        public CollaboratorMapper()
        {
            CreateMap<Collaborator, CollaboratorResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
