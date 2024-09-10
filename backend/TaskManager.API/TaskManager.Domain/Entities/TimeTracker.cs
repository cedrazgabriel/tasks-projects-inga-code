using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.Entities
{
    public class TimeTracker : BaseEntity
    {
        private TimeTracker() { }

        public TimeTracker(DateTime startDate, string timeZoneId, Guid taskId, Guid collaboratorId)
        {
            StartDate = startDate.ToUniversalTime();
            TimeZoneId = timeZoneId;
            CreatedAt = DateTime.UtcNow;
            TaskId = taskId;
            CollaboratorId = collaboratorId;
        }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TimeZoneId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; } = null;
        public Guid TaskId { get; set; }
        public TaskProject Task { get; set; }

        public Guid CollaboratorId { get; set; }
        public Collaborator Collaborator { get; set; }
    }
}
