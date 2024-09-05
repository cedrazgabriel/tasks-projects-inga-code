using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.UseCases.Errors
{
    public class UserAlreadyExistsError : BaseError
    {
        public UserAlreadyExistsError() : base("User already exists.", 409)
        {
        }
    }
}
