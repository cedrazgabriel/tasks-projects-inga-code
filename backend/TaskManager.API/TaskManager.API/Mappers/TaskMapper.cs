using AutoMapper;
using TaskManager.API.DTO.Response;
using TaskManager.Domain.Entities;

namespace TaskManager.API.Mappers
{
    public class TaskMapper : Profile
    {
        public TaskMapper()
        {
            CreateMap<TaskProject, TaskResponse>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
           .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId.ToString()))
           .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")))
           .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.HasValue ? src.UpdatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : null))
           .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
           .ForMember(dest => dest.TotalTimeSpent, opt => opt.MapFrom(src => src.TimeTrackers
               .Select(tracker => (tracker.EndDate ?? DateTime.Now) - tracker.StartDate)
               .Aggregate(TimeSpan.Zero, (total, current) => total + current)
               .ToString(@"hh\:mm\:ss")));
        }
    }
}
