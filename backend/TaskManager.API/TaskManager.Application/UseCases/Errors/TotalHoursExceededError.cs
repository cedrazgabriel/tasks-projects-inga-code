using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.UseCases.Errors
{
    public class TotalHoursExceededError : BaseError
    {
        public TotalHoursExceededError() : base("Total time tracked in a single day cannot exceed 24 hours.", 400)
        {
        }
    }
}
