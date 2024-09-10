using System;
using System.Threading.Tasks;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Domain.Entities;
using TaskManager.Application.UseCases.Errors;

namespace TaskManager.Application.UseCases.TimeTrackers
{
    public class StopTimeTrackerUseCase(ITimeTrackerRepository timeTrackerRepository, ITaskRepository taskRepository)
    {
      
        public async Task<TimeTracker> Execute(Guid timeTrackerId)
        {

            //Verifica se o timetracker existe
            var timeTracker = await timeTrackerRepository.Get(timeTrackerId);

            if (timeTracker is null)
            {
                throw new ResourceNotFoundError();
            }

            // Verifica se a tarefa existe
            var task = await taskRepository.GetTaskByIdAsync(timeTracker.TaskId);

            if (task is null)
            {
                throw new ResourceNotFoundError();
            }

        
            var timeTrackers = await timeTrackerRepository.GetTimeTrackersByTaskIdAsync(task.Id);

            // Encontra o time tracker que está em andamento (sem EndDate definido)
            var timeTrackerInProgress = timeTrackers.FirstOrDefault(tt => !tt.EndDate.HasValue);

            if (timeTrackerInProgress is null)
            {
                throw new BaseError("Não existe timetracker em andamento", 400);
            }

            var endDate = DateTime.UtcNow;

            if (endDate <= timeTrackerInProgress.StartDate)
            {
                throw new InvalidTimeIntervalError();
            }

       
            timeTrackerInProgress.SetEndDate(endDate);

 
            await timeTrackerRepository.UpdateAsync(timeTrackerInProgress);

            return timeTrackerInProgress;
        }
    }
}
