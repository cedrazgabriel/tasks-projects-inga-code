using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.UseCases.Errors
{
    public class ResourceNotFoundError : BaseError
    {
        public ResourceNotFoundError() : base("Resource not found.", 404)
        {
        }
    }
}
