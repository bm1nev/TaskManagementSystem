using TaskManagementSystem.Domain.Common;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Domain.Entities;

public class ProjectMember : BaseEntity
{
    public Guid ProjectId { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public ProjectRole Role { get; private set; } = ProjectRole.Member;
    
    public Guid? InvitedByUserId { get; private set; }
    
    public string? Note { get; private set; }

    public DateTime JoinedAtUtc { get; private set; } = DateTime.UtcNow;
    
    private ProjectMember() { }

    public ProjectMember(
        Guid projectId, 
        Guid userId,
        ProjectRole role = ProjectRole.Member,
        Guid? invitedByUserId = null,
        string? note = null)
    {
        if (projectId == Guid.Empty)
            throw new ArgumentException("ProjectId is required.", nameof(projectId));
        
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId is required.", nameof(userId));
        
        if (invitedByUserId == Guid.Empty)
            throw new ArgumentException("InvitedByUserId cannot be empty Guid.", nameof(invitedByUserId));
        
        ProjectId = projectId;
        UserId = userId;

        Role = role;
        InvitedByUserId = invitedByUserId;
        Note = NormalizeOptional(note);
    }

    public void ChangeRole(ProjectRole newRole)
    {
        Role = newRole;
    }

    public void SetNote(string? note)
    {
        Note = NormalizeOptional(note);
    }

    private static string? NormalizeOptional(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return value.Trim();
    }
}