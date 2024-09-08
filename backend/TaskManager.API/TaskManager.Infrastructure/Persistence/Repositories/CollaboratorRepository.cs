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
    public class CollaboratorRepository(TaskManagerDbContext dbContext) : ICollaboratorRepository
    {
        public async Task CreateAsync(Collaborator collaborator)
        {
            await dbContext.Collaborators.AddAsync(collaborator);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Collaborator> GetByUserIdAsync(Guid userId)
        {
            return await dbContext.Collaborators.Where(c => c.UserId == userId).FirstOrDefaultAsync();
        }
    }
}
