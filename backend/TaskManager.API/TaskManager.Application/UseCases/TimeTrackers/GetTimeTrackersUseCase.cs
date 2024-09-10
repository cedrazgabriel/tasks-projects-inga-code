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
    public class GetTimeTrackersByTaskIdUseCase(ITimeTrackerRepository timeTrackerRepository)
    {
        public async Task<PaginatedResult<TimeTracker>> Execute(Guid taskId, int page, int pageSize)
        {
            var timeTrackers = await timeTrackerRepository.GetTimeTrackersWithFiltersPaginatedAsync(taskId, page, pageSize);

            return timeTrackers;
        }
    }
}
