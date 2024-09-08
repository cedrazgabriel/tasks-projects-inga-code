using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.UseCases.Errors
{
    public class InvalidTimeIntervalError : BaseError
    {
        public InvalidTimeIntervalError() : base("The start date must be less than or equal to the end date.", 400)
        {
        }
    }
}
