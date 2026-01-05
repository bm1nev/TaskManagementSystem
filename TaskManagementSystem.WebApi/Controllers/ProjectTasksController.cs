using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.DTOs.Tasks;
using TaskManagementSystem.Application.Services;
using TaskManagementSystem.WebApi.Security;

namespace TaskManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/projects/{projectId:guid}/tasks")]
[Authorize]
public sealed class ProjectTasksController : ControllerBase
{
    private readonly TaskService _tasks;
    
    public ProjectTasksController(TaskService tasks)
    {
        _tasks = tasks;
    }

    [HttpPost]
    public async Task<ActionResult<object>> Create(Guid projectId, CreateTaskRequestDto request)
    {
        var currentUserId = User.GetUserId();

        var taskId = await _tasks.CreateAsync(projectId, currentUserId, request);
        
        return Ok(new { taskId });
    }

    [HttpGet]
    public async Task<ActionResult<List<TaskListItemDto>>> List(Guid projectId)
    {
        var currentUserId = User.GetUserId();
        
        var list = await _tasks.GetForProjectAsync(projectId, currentUserId);
        
        return Ok(list);
    }
}