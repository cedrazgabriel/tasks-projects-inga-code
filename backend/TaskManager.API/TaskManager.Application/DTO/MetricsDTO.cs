using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.DTO
{
    public class MetricsDTO
    {
        public string TotalHoursSpentThisMonth { get; set; }
        public string TotalHoursSpentToday { get; set; }
        public string TotalHoursSpentThisWeek { get; set; }
    }
}
