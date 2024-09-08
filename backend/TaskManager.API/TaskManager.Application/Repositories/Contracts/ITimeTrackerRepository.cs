using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Repositories.Contracts
{
    public interface ITimeTrackerRepository
    {
        public Task<List<TimeTracker>> GetTimeTrackersByTaskIdAsync(Guid taskId);
        public Task CreateAsync(TimeTracker timeTracker);
    }
}
