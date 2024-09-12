using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Repositories.Contracts
{
    public interface IUserRepository
    {
        public System.Threading.Tasks.Task CreateAsync(User user);
        public Task<User?> FindByUserNameAsync(string username);
    }
}
