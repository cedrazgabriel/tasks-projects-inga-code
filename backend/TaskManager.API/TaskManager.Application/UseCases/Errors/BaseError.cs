using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.UseCases.Errors
{
    public class BaseError : Exception
    {
        public int StatusCode { get; }

        public BaseError(string message, int statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
