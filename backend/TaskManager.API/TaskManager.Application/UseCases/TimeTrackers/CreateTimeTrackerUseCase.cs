using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Domain.Entities;
using TaskManager.Application.UseCases.Errors;

namespace TaskManager.Application.UseCases.TimeTrackers
{
    public class InitTimeTrackerUseCase
    {
        private readonly ITaskRepository taskRepository;
        private readonly ITimeTrackerRepository timeTrackerRepository;
        private readonly ICollaboratorRepository collaboratorRepository;

        public InitTimeTrackerUseCase(ITaskRepository taskRepository, ITimeTrackerRepository timeTrackerRepository, ICollaboratorRepository collaboratorRepository)
        {
            this.taskRepository = taskRepository;
            this.timeTrackerRepository = timeTrackerRepository;
            this.collaboratorRepository = collaboratorRepository;
        }

        public async Task<TimeTracker> Execute(Guid userId, Guid taskId, DateTime startDate, string timeZoneId, DateTime? endDate = null)
        {
            // Verifica se a tarefa existe
            var task = await taskRepository.GetTaskByIdAsync(taskId);
            if (task is null)
            {
                throw new ResourceNotFoundError();
            }

            // Se endDate for nulo, ignorar validação de intervalo
            if (endDate.HasValue && startDate > endDate.Value)
            {
                throw new InvalidTimeIntervalError();
            }

            // Obtém todos os time trackers da tarefa
            var timeTrackers = await timeTrackerRepository.GetTimeTrackersByTaskIdAsync(task.Id);

            foreach (var existingTimeTracker in timeTrackers)
            {
                // Ajuste para timezone se necessário
                var adjustedStartDate = existingTimeTracker.StartDate.AddHours(-3);
                var adjustedEndDate = existingTimeTracker.EndDate?.AddHours(-3);

                // Se o time tracker existente não tem endDate, ele está em andamento
                if (!existingTimeTracker.EndDate.HasValue)
                {
                    throw new ConflictingHourError();
                }

                // Valida sobreposição de intervalos de tempo
                if (endDate.HasValue)
                {
                    if ((startDate >= adjustedStartDate && startDate < adjustedEndDate) ||
                        (endDate > adjustedStartDate && endDate <= adjustedEndDate))
                    {
                        throw new ConflictingHourError();
                    }
                }
            }

            // Calcular as horas totais no dia
            var totalHoursInDay = timeTrackers
                .Where(tt => tt.StartDate.Date == startDate.Date)
                .Sum(tt => (tt.EndDate.HasValue ? (tt.EndDate.Value - tt.StartDate).TotalHours : 0));

            var newTimeTrackerHours = endDate.HasValue ? (endDate.Value - startDate).TotalHours : 0;

            if (totalHoursInDay + newTimeTrackerHours > 24)
            {
                throw new TotalHoursExceededError();
            }

            // Verifica se o colaborador existe
            var collaborator = await collaboratorRepository.GetByUserIdAsync(userId);

            // Cria um novo time tracker
            var timeTracker = new TimeTracker(startDate, timeZoneId, taskId, collaborator.Id);

            await timeTrackerRepository.CreateAsync(timeTracker);

            return timeTracker;
        }
    }
}
