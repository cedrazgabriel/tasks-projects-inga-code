using AutoMapper;
using TaskManager.API.DTO.Response;
using TaskManager.Application.DTO;
using TaskManager.Domain.Entities;

namespace TaskManager.API.Mappers
{
    public class TimeTrackerMapper : Profile
    {
        public TimeTrackerMapper()
        {
            CreateMap<TimeTracker, TimeTrackerResponse>()
             .ForMember(dest => dest.CollaboratorId, opt => opt.MapFrom(src => src.CollaboratorId.ToString()))
             .ForMember(dest => dest.CollaboratorName, opt => opt.MapFrom(src => src.Collaborator.Name))
             .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")))
             .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.HasValue ? src.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : null))
             .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString("yyyy-MM-dd HH:mm:ss")))
             .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId.ToString()))
             .ForMember(dest => dest.TaskName, opt => opt.MapFrom(src => src.Task.Name))
             .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.Task.Project.Id.ToString()))
             .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Task.Project.Name));

            CreateMap<MetricsDTO, MetricsResponse>()
                       .ForMember(dest => dest.TotalHoursSpentToday, opt => opt.MapFrom(src => src.TotalHoursSpentToday))
                       .ForMember(dest => dest.TotalHoursSpentThisWeek, opt => opt.MapFrom(src => src.TotalHoursSpentThisWeek))
                       .ForMember(dest => dest.TotalHoursSpentThisMonth, opt => opt.MapFrom(src => src.TotalHoursSpentThisMonth));

        }
    }
}
