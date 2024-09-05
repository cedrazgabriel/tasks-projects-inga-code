using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
   
            builder.ToTable("users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.UserName)
                .HasColumnName("username")
                .IsRequired();

            builder.HasIndex(u => u.UserName)
                .IsUnique();

            builder.Property(u => u.Password)
                .HasColumnName("password")
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasDefaultValueSql("NOW()"); 

            builder.Property(u => u.UpdatedAt)
                .HasColumnName("updated_at");

            builder.Property(u => u.DeletedAt)
                .HasColumnName("deleted_at");

            
            builder.HasOne(u => u.Collaborator)
                .WithOne()
                .HasForeignKey<Collaborator>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
