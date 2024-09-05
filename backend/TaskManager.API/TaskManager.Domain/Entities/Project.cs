
namespace TaskManager.Domain.Entities
{
    public class Project : BaseEntity
    {

        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ICollection<Task>? Tasks { get; set; }
    }
}
