using TaskManagementSystem.Application.DTOs.Tasks;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Enums;
using TaskStatus = TaskManagementSystem.Domain.Enums.TaskStatus;

namespace TaskManagementSystem.Application.Services;

public sealed class TaskService
{
    private readonly ITaskRepository _tasks;
    private readonly IProjectRepository _projects;

    public TaskService(ITaskRepository tasks, IProjectRepository projects)
    {
        _tasks = tasks;
        _projects = projects;
    }

    public async Task<Guid> CreateAsync(
        Guid projectId,
        Guid currentUserId,
        CreateTaskRequestDto request)
    {
        // 1) is project exists?
        _ = await _projects.GetProjectAsync(projectId)
            ?? throw new InvalidOperationException($" Project {projectId} not found. ");
        
        // 2) is user a member?
        var member = await _projects.GetMemberAsync(projectId, currentUserId)
                     ?? throw new InvalidOperationException($" You are not member in project {projectId}. ");
        
        // 3) permission 
        if (member.Role is not (ProjectRole.Owner or ProjectRole.Manager))
            throw new InvalidOperationException(" Insufficient permissions. ");
        
        // 4) create task
        var task = new TaskItem(
            title: request.Title,
            projectId: projectId,
            description: request.Description,
            dueDateUtc: request.DueDateUtc
        );
        
        await _tasks.AddAsync(task);
        await _tasks.SaveChangesAsync();
        
        return task.Id;
    }

    public async Task<List<TaskListItemDto>> GetForProjectAsync(
        Guid projectId,
        Guid currentUserId)
    {
        // view = member
        var member = await _projects.GetMemberAsync(projectId, currentUserId);
        if (member is null)
            throw new InvalidOperationException(" You are not a project member. ");
        
        var tasks = await _tasks.GetForProjectAsync(projectId);
            
        return tasks.Select(t => new TaskListItemDto
        {
            Id = t.Id,
            Title = t.Title,
            Status = t.Status,
            DueDateUtc = t.DueDateUtc
        }).ToList();
    }

    public async Task AssignUserAsync(
        Guid taskId,
        Guid currentUserId,
        AssignUserToTaskRequestDto request)
    {
        var (projectId, exists) = await _tasks.GetProjectInfoAsync(taskId);
        if (!exists)
            throw new InvalidOperationException($"Task {taskId} not found.");

        var member = await _projects.GetMemberAsync(projectId, currentUserId)
                     ?? throw new InvalidOperationException("You are not a project member.");

        if (member.Role is not (ProjectRole.Owner or ProjectRole.Manager))
            throw new InvalidOperationException("Insufficient permissions.");

        var assignment = new TaskAssignment(taskId, request.UserId);

        await _tasks.AddAssignmentAsync(assignment);
        await _tasks.SaveChangesAsync();
    }

    public async Task UnassignUserAsync(
        Guid taskId,
        Guid currentUserId,
        Guid userId)
    {
        var task = await _tasks.GetByIdAsync(taskId)
            ?? throw new InvalidOperationException($" Task {taskId} not found. ");
        
        var member = await _projects.GetMemberAsync(task.ProjectId, currentUserId)
            ?? throw new InvalidOperationException("You are not a project member. ");
        
        if (member.Role is not (ProjectRole.Owner or ProjectRole.Manager))
            throw new InvalidOperationException(" Insufficient permissions. ");
        
        task.UnassignUser(userId);
        await _tasks.SaveChangesAsync();
    }

    public async Task ChangeStatusAsync(
        Guid taskId,
        Guid currentUserId,
        ChangeTaskStatusRequestDto request)
    {
        var task = await _tasks.GetByIdAsync(taskId)
            ?? throw new InvalidOperationException($" Task {taskId} not found. ");
        
        // member of the project
        var membership = await _projects.GetMemberAsync(task.ProjectId, currentUserId)
            ?? throw new InvalidOperationException("You are not a project member. ");
        
        var isPrivileget = membership.Role is ProjectRole.Owner or ProjectRole.Manager;
        var isAssignee = task.IsAssignedTo(currentUserId);
        
        if (!isPrivileget && !isAssignee)
            throw new InvalidOperationException("Insufficient permissions.");
        
        if (!Enum.TryParse<TaskStatus>(request.Status, out var newStatus))
            throw new InvalidOperationException($"Status {request.Status} is not valid.");
        
        task.ChangeStatus(newStatus);
        await _tasks.SaveChangesAsync();
    }
}