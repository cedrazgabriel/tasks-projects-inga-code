using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence.Repositories
{
    public class TimeTrackerRepository(TaskManagerDbContext dbContext) : ITimeTrackerRepository
    {
        public async Task<List<TimeTracker>> GetTimeTrackersByTaskIdAsync(Guid taskId)
        {
            return await dbContext.TimeTrackers
                                   .Where(tt => tt.TaskId == taskId && tt.DeletedAt == null)
                                   .ToListAsync();
        }

        public async Task CreateAsync(TimeTracker timeTracker)
        {
            await dbContext.AddAsync(timeTracker);
            await dbContext.SaveChangesAsync();
        }
    }
}
