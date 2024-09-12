using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API.DTO.Response;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Repositories.Contracts
{
    public interface ITimeTrackerRepository
    {
        public Task<TimeTracker> Get(Guid id);
        public Task<List<TimeTracker>> GetTimeTrackersByTaskIdAsync(Guid taskId);
        public Task<List<TimeTracker>> GetByCollaboratorIdAsync(Guid collaboratorId);
        Task<PaginatedResult<TimeTracker>> GetTimeTrackersWithFiltersPaginatedAsync(Guid taskId, int page, int pageSize, Guid? collaboratorId);
        public Task CreateAsync(TimeTracker timeTracker);
        public Task UpdateAsync(TimeTracker timeTracker);
    }
}
