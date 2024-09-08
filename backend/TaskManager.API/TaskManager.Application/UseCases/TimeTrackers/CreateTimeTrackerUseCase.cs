using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Domain.Entities;
using TaskManager.Application.UseCases.Errors;

namespace TaskManager.Application.UseCases.TimeTrackers
{
    public class CreateTimeTrackerUseCase(ITaskRepository taskRepository, ITimeTrackerRepository timeTrackerRepository)
    {
        public async Task<TimeTracker> Execute(Guid collaboratorId, Guid taskId, DateTime startDate, DateTime endDate, string timeZoneId)
        {
            var task = await taskRepository.GetTaskByIdAsync(taskId);

            if (task is null)
            {
                throw new ResourceNotFoundError();
            }

            if (startDate > endDate)
            {
                throw new InvalidTimeIntervalError();
            }

            var timeTrackers = await timeTrackerRepository.GetTimeTrackersByTaskIdAsync(task.Id);

            
            foreach (var existingTimeTracker in timeTrackers)
            {
                if ((startDate >= existingTimeTracker.StartDate && startDate < existingTimeTracker.EndDate) ||
                    (endDate > existingTimeTracker.StartDate && endDate <= existingTimeTracker.EndDate))
                {
                    throw new ConflictingHourError();
                }
            }

            var totalHoursInDay = timeTrackers
                .Where(tt => tt.StartDate.Date == startDate.Date)
                .Sum(tt => (tt.EndDate - tt.StartDate).TotalHours);

            var newTimeTrackerHours = (endDate - startDate).TotalHours;

            if (totalHoursInDay + newTimeTrackerHours > 24)
            {
                throw new TotalHoursExceededError();
            }

            var timeTracker = new TimeTracker(startDate, endDate, timeZoneId, taskId, collaboratorId);
           
            await timeTrackerRepository.CreateAsync(timeTracker);

            return timeTracker;
        }
    }
}
