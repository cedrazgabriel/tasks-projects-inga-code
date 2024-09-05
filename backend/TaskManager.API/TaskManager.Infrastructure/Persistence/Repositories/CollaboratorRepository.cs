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
        public async System.Threading.Tasks.Task CreateAsync(Collaborator collaborator)
        {
            await dbContext.Collaborators.AddAsync(collaborator);
            await dbContext.SaveChangesAsync();
        }
    }
}
