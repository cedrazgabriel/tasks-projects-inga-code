﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence.Configurations
{
    public class CollaboratorConfigurations : IEntityTypeConfiguration<Collaborator>
    {
        public void Configure(EntityTypeBuilder<Collaborator> builder)
        {
            
            builder.ToTable("collaborators");

            builder.HasKey(c => c.Id);

          
            builder.Property(c => c.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(c => c.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(c => c.UpdatedAt)
                .HasColumnName("updated_at");
                

            builder.Property(c => c.DeletedAt)
                .HasColumnName("deleted_at");

     
            builder.Property(c => c.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.HasOne<User>()
                .WithOne(u => u.Collaborator)
                .HasForeignKey<Collaborator>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.TimeTrackers)
                .WithOne()
                .HasForeignKey(t => t.CollaboratorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
