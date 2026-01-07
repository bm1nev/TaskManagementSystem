
using TaskManagementSystem.Domain.Common;
using TaskStatus = TaskManagementSystem.Domain.Enums.TaskStatus;

namespace TaskManagementSystem.Domain.Entities;

public class TaskItem : BaseEntity
{
    public string Title { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public TaskStatus Status { get; private set; } = TaskStatus.Open;

    public Guid ProjectId { get; private set; }

    public DateTime? DueDateUtc { get; private set; }

    private readonly List<TaskAssignment> _assignments = new();

    public IReadOnlyCollection<TaskAssignment> Assignments => _assignments.AsReadOnly();

    private TaskItem() { }

    public TaskItem(string title, Guid projectId, string? description = null, DateTime? dueDateUtc = null)
    {
        SetTitle(title);

        if (projectId == Guid.Empty)
            throw new ArgumentException("ProjectId is required.", nameof(projectId));

        ProjectId = projectId;
        Description = NormalizeOptional(description);
        DueDateUtc = NormalizeDueDateUtc(dueDateUtc);

        Status = TaskStatus.Open;
    }

    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Task title is required.", nameof(title));

        Title = title.Trim();
    }

    public void SetDescription(string? description)
    {
        Description = NormalizeOptional(description);
    }

    public void SetDueDateUtc(DateTime? dueDateUtc)
    {
        DueDateUtc = NormalizeDueDateUtc(dueDateUtc);
    }

    public void ChangeStatus(TaskStatus newStatus)
    {
        if (Status == TaskStatus.Open && newStatus == TaskStatus.Done)
            throw new InvalidOperationException("Cannot move directly from Open to Done.");

        Status = newStatus;
    }

    public void AssignUser(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId is required.", nameof(userId));

        if (_assignments.Any(a => a.UserId == userId))
            return;

        _assignments.Add(new TaskAssignment(Id, userId));
    }

    public void UnassignUser(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId is required.", nameof(userId));

        var assignment = _assignments.FirstOrDefault(a => a.UserId == userId);
        if (assignment is null)
            return;

        _assignments.Remove(assignment);
    }

    public bool IsAssignedTo(Guid userId)
    {
        if (userId == Guid.Empty)
            return false;

        return _assignments.Any(a => a.UserId == userId);
    }

    private static string? NormalizeOptional(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return value.Trim();
    }

    private static DateTime? NormalizeDueDateUtc(DateTime? dueDateUtc)
    {
        if (dueDateUtc is null)
            return null;

        return DateTime.SpecifyKind(dueDateUtc.Value, DateTimeKind.Utc);
    }
}
