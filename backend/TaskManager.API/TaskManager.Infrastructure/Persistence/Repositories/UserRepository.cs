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
    public class UserRepository(TaskManagerDbContext dbContext) : IUserRepository
    {
        public async Task CreateAsync(User user)
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }
        public async Task<User> FindByUserNameAsync(string username)
        {
            return await dbContext.Users.FirstOrDefaultAsync(user => user.UserName == username);
        }
    }
}
