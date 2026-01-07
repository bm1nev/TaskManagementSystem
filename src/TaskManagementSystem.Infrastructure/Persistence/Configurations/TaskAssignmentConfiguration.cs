using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Infrastructure.Persistence.Configurations;

public sealed class TaskAssignmentConfiguration : IEntityTypeConfiguration<TaskAssignment>
{
    public void Configure(EntityTypeBuilder<TaskAssignment> builder)
    {
        builder.ToTable("TaskAssignments");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TaskId)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();
        
        builder.Property(x => x.AssignedAtUtc)
            .IsRequired();
        
        builder.HasIndex(x => new { x.TaskId, x.UserId })
            .IsUnique();
    }
}