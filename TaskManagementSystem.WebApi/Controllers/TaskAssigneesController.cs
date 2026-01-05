using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.DTOs.Tasks;
using TaskManagementSystem.Application.Services;
using TaskManagementSystem.WebApi.Security;

namespace TaskManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/tasks/{taskId:guid}/assignees")]
[Authorize]
public sealed class TaskAssigneesController : ControllerBase
{
    private readonly TaskService _tasks;
    
    public TaskAssigneesController(TaskService tasks)
    {
        _tasks = tasks;
    }

    [HttpPost]
    public async Task<IActionResult> Assign(Guid taskId, AssignUserToTaskRequestDto request)
    {
        var currentUserId = User.GetUserId();
        
        await _tasks.AssignUserAsync(taskId, currentUserId, request);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Unassign(Guid taskId, Guid userId)
    {
        var currentUserId = User.GetUserId();
        
        await _tasks.UnassignUserAsync(taskId, currentUserId, userId);
        return NoContent();
    }
}