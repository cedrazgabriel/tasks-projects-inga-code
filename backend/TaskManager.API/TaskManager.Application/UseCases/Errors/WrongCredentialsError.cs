using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.UseCases.Errors
{
    public class WrongCredentialsError : BaseError
    {
        public WrongCredentialsError() : base("Credentials are is not valid", 401)
        {
        }
    }
}       
