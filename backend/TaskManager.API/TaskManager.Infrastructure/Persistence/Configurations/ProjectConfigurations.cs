using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence.Configurations
{
    public class ProjectConfigurations : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            
            builder.ToTable("projects");


            builder.HasKey(p => p.Id);

          
            builder.Property(p => p.Name)
                .HasColumnName("name")
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                 .HasDefaultValueSql("NOW()");

            builder.Property(p => p.UpdatedAt)
                .HasColumnName("updated_at");

            builder.Property(p => p.DeletedAt)
                .HasColumnName("deleted_at");

            builder.HasMany(p => p.Tasks)
                .WithOne()
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
