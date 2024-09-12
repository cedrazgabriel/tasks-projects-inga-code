using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.DTO
{
    public class MetricsDTO
    {
        public string TotalHoursSpentThisMonth { get; set; } = string.Empty;
        public string TotalHoursSpentToday { get; set; } = string.Empty;
        public string TotalHoursSpentThisWeek { get; set; } = string.Empty;
    }
}
