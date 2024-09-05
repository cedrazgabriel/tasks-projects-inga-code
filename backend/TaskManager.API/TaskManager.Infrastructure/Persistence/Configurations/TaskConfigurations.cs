using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence.Configurations
{
    public class TaskConfigurations : IEntityTypeConfiguration<Domain.Entities.Task>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Task> builder)
        {
           
            builder.ToTable("tasks");

          
            builder.HasKey(t => t.Id);

         
            builder.Property(t => t.Name)
                .HasColumnName("name")
                .IsRequired();

            builder.Property(t => t.Description)
                .HasColumnName("description");

            builder.Property(t => t.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(t => t.UpdatedAt)
                .HasColumnName("updated_at");

            builder.Property(t => t.DeletedAt)
                .HasColumnName("deleted_at");

          
            builder.Property(t => t.ProjectId)
                .HasColumnName("project_id")
                .IsRequired();

            builder.HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.TimeTrackers)
                .WithOne()
                .HasForeignKey(tt => tt.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
