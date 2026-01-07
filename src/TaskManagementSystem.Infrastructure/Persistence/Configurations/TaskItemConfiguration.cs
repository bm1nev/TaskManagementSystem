using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Infrastructure.Persistence.Configurations;

public sealed class TaskItemConfiguration
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("Tasks");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Title)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(2048);

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.ProjectId)
            .IsRequired();

        builder.Property(x => x.DueDateUtc);
        
        builder.Property(x=> x.CreatedAtUtc)
            .IsRequired();
        
        builder.HasMany<TaskAssignment>("_assigments")
            .WithOne()
            .HasForeignKey(a => a.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Navigation(x=> x.Assignments)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}