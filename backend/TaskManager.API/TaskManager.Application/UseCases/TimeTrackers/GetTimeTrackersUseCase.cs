using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API.DTO.Response;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.TimeTrackers
{
    public class GetTimeTrackersUseCase(ITimeTrackerRepository timeTrackerRepository)
    {
        public async Task<PaginatedResult<TimeTracker>> Execute(Guid? projectId, Guid? collaboratorId, int page, int pageSize)
        {
            var timeTrackers = await timeTrackerRepository.GetTasksWithFiltersAsync(projectId, collaboratorId);

            return timeTrackers;
        }
    }
}
