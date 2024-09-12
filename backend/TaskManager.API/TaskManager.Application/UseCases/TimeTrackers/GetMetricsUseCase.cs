using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Domain.Entities;
using TaskManager.Application.UseCases.Errors;
using TaskManager.Application.DTO;

namespace TaskManager.Application.UseCases.TimeTrackers
{
    public class GetMetricsUseCase(ITimeTrackerRepository timeTrackerRepository, ICollaboratorRepository collaboratorRepository)
    {

        public async Task<MetricsDTO> Execute(Guid userId)
        {
            var collaborator = await collaboratorRepository.GetByUserIdAsync(userId);

            if (collaborator == null)
            {
                throw new ResourceNotFoundError();
            }

            var timeTrackers = await timeTrackerRepository.GetByCollaboratorIdAsync(collaborator.Id);

            if (timeTrackers == null || !timeTrackers.Any())
            {
                return new MetricsDTO
                {
                    TotalHoursSpentToday = "0h 0m",
                    TotalHoursSpentThisWeek = "0h 0m",
                    TotalHoursSpentThisMonth = "0h 0m"
                };
            }

            // Cálculos de horas gastas no mês, semana e dia
            var today = DateTime.UtcNow.Date;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek); // Considerando domingo como o início da semana
            var startOfMonth = new DateTime(today.Year, today.Month, 1); // Primeiro dia do mês

            var totalHoursSpentToday = timeTrackers
                .Where(t => t.StartDate.Date == today && t.EndDate.HasValue)
                .Sum(t => (t.EndDate.Value - t.StartDate).TotalHours);

            var totalHoursSpentThisWeek = timeTrackers
                .Where(t => t.StartDate.Date >= startOfWeek && t.EndDate.HasValue)
                .Sum(t => (t.EndDate.Value - t.StartDate).TotalHours);

            var totalHoursSpentThisMonth = timeTrackers
                .Where(t => t.StartDate.Date >= startOfMonth && t.EndDate.HasValue)
                .Sum(t => (t.EndDate.Value - t.StartDate).TotalHours);

            // Convertendo para formato de horas e minutos
            return new MetricsDTO
            {
                TotalHoursSpentToday = ConvertHoursToHoursAndMinutes(totalHoursSpentToday),
                TotalHoursSpentThisWeek = ConvertHoursToHoursAndMinutes(totalHoursSpentThisWeek),
                TotalHoursSpentThisMonth = ConvertHoursToHoursAndMinutes(totalHoursSpentThisMonth)
            };
        }

        // Função de conversão
        private string ConvertHoursToHoursAndMinutes(double totalHours)
        {
            int hours = (int)totalHours;
            int minutes = (int)((totalHours - hours) * 60);
            return $"{hours}h {minutes}m";
        }

    }
}
