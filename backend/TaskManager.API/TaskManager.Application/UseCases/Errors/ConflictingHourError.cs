using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.UseCases.Errors
{
    public class ConflictingHourError : BaseError    
    {
        public ConflictingHourError() : base("Time interval overlaps with an existing time tracker.", 409)
        {
        }
    }
}
