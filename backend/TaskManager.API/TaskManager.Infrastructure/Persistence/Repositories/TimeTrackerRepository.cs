﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API.DTO.Response;
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

        public async Task<PaginatedResult<TimeTracker>> GetTimeTrackersWithFiltersPaginatedAsync(
     Guid? projectId, Guid? collaboratorId, int page, int pageSize)
        {
            var query = dbContext.TimeTrackers
                .Include(tt => tt.Task)
                .ThenInclude(t => t.Project)
                .Include(tt => tt.Collaborator)
                .AsQueryable();

            if (projectId.HasValue)
            {
                query = query.Where(tt => tt.Task.ProjectId == projectId.Value);
            }

            if (collaboratorId.HasValue)
            {
                query = query.Where(tt => tt.CollaboratorId == collaboratorId.Value);
            }

            // Contagem total de registros
            var totalRecords = await query.CountAsync();

            // Aplicação da paginação
            var paginatedItems = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

          
            return new PaginatedResult<TimeTracker>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Items = paginatedItems 
            };
        }


    }
}
