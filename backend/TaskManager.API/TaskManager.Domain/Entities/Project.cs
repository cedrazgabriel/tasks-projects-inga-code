
namespace TaskManager.Domain.Entities
{
    public class Project : BaseEntity
    {
        private Project() { }

        public Project(string name)
        {
            Name = name;
        }

        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ICollection<TaskProject>? Tasks { get; set; }
    }
}
