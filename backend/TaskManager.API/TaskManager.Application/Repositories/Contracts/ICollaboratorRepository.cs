using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Repositories.Contracts
{
    public interface ICollaboratorRepository
    {
        public Task<List<Collaborator>> GetAll();
        public Task CreateAsync(Collaborator collaborator);
        public Task<Collaborator> GetByUserIdAsync(Guid userId);
    }
}
