using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence.Configurations
{
    public class TimeTrackerConfigurations : IEntityTypeConfiguration<TimeTracker>
    {
        public void Configure(EntityTypeBuilder<TimeTracker> builder)
        {
          
            builder.ToTable("time_trackers");

            builder.HasKey(tt => tt.Id);
         
            builder.Property(tt => tt.StartDate)
                .HasColumnName("start_date")
                .IsRequired();

            builder.Property(tt => tt.EndDate)
                .HasColumnName("end_date")
                .IsRequired();

            builder.Property(tt => tt.TimeZoneId)
                .HasColumnName("time_zone_id")
                .IsRequired();

            builder.Property(tt => tt.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(tt => tt.UpdatedAt)
                .HasColumnName("updated_at");

            builder.Property(tt => tt.DeletedAt)
                .HasColumnName("deleted_at");

         
            builder.Property(tt => tt.TaskId)
                .HasColumnName("task_id")
                .IsRequired();

            builder.HasOne(tt => tt.Task)
                .WithMany(t => t.TimeTrackers)
                .HasForeignKey(tt => tt.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            
            builder.Property(tt => tt.CollaboratorId)
                .HasColumnName("collaborator_id")
                .IsRequired();

            builder.HasOne(tt => tt.Collaborator)
                .WithMany(c => c.TimeTrackers)
                .HasForeignKey(tt => tt.CollaboratorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
