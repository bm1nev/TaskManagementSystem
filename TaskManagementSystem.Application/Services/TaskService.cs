using TaskManagementSystem.Application.DTOs.Tasks;
using TaskManagementSystem.Application.Exceptions;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Enums;
using TaskStatus = TaskManagementSystem.Domain.Enums.TaskStatus;

namespace TaskManagementSystem.Application.Services;

public sealed class TaskService
{
    private readonly ITaskRepository _tasks;
    private readonly IProjectRepository _projects;
    private readonly ProjectAccessService _access;

    public TaskService(ITaskRepository tasks, IProjectRepository projects, ProjectAccessService access)
    {
        _tasks = tasks;
        _projects = projects;
        _access = access;
    }

    public async Task<Guid> CreateAsync(
        Guid projectId,
        Guid currentUserId,
        CreateTaskRequestDto request)
    {
        await _access.RequireProjectAsync(projectId);
        
        await _access.RequireOwnerOrManagerAsync(projectId, currentUserId);
        
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
        await _access.RequireMemberAsync(projectId, currentUserId);

        return await _tasks.GetForProjectWithAssigneesAsync(projectId);
    }

    public async Task AssignUserAsync(
        Guid taskId,
        Guid currentUserId,
        AssignUserToTaskRequestDto request)
    {
        var task = await _tasks.GetByIdAsync(taskId)
            ?? throw new NotFoundException($" Task {taskId} not found. ");
        
        await _access.RequireOwnerOrManagerAsync(task.ProjectId, currentUserId);

        task.AssignUser(request.UserId);
        await _tasks.SaveChangesAsync();
    }

    public async Task UnassignUserAsync(
        Guid taskId,
        Guid currentUserId,
        Guid userId)
    {
        var task = await _tasks.GetByIdAsync(taskId)
            ?? throw new NotFoundException($" Task {taskId} not found. ");
        
        await _access.RequireOwnerOrManagerAsync(task.ProjectId, currentUserId);
        
        task.UnassignUser(userId);
        await _tasks.SaveChangesAsync();
    }

    public async Task ChangeStatusAsync(
        Guid taskId,
        Guid currentUserId,
        ChangeTaskStatusRequestDto request)
    {
        var task = await _tasks.GetByIdAsync(taskId)
            ?? throw new NotFoundException($" Task {taskId} not found. ");
        
        await _access.RequireMemberAsync(task.ProjectId, currentUserId);

        var isPrivileged = await _access.IsOwnerOrManagerAsync(task.ProjectId, currentUserId);
        var isAssignee = task.IsAssignedTo(currentUserId);
        
        if (!isPrivileged && !isAssignee)
            throw new ForbiddenException("Insufficient permissions.");
        
        if (!Enum.TryParse<TaskStatus>(request.Status, true, out var newStatus))
            throw new ValidationException($"Status {request.Status} is not valid.");
        
        task.ChangeStatus(newStatus);
        await _tasks.SaveChangesAsync();
    }
}