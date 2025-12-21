using TaskManagementSystem.Domain.Common;

namespace TaskManagementSystem.Domain.Entities;

public class TaskAssignment : BaseEntity
{
    public Guid TaskId { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public DateTime AssignedAtUtc { get; private set; } = DateTime.UtcNow;
    
    private TaskAssignment() { }

    public TaskAssignment(Guid taskId, Guid userId)
    {
        if (taskId == Guid.Empty)
            throw new ArgumentException("TaskId is required", nameof(taskId));
        
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId is required", nameof(userId));
        
        TaskId = taskId;
        UserId = userId;
    }
}