using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Cryptograph.Contracts;
using TaskManager.Application.Repositories.Contracts;
using TaskManager.Application.Services;
using TaskManager.Application.UseCases.Errors;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.UseCases.Users
{
    public class AuthenticateUseCase(IUserRepository usersRepository, IHashCompare hashCompare, IAuthService authService)
    {
        public async Task<string> Execute(string username, string password)
        {
            var user = await usersRepository.FindByUserNameAsync(username);

            if (user is null)
            {
                throw new WrongCredentialsError();
            }

            var isPasswordValid = await hashCompare.CompareAsync(password, user.Password);

            if (!isPasswordValid)
            {
                throw new WrongCredentialsError();
            }

            var jwtToken = await authService.GenerateJwtToken(user.UserName);

            return jwtToken;
        }
    }
}
