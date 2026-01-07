using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.DTOs.Tasks;
using TaskManagementSystem.Application.Services;
using TaskManagementSystem.WebApi.Security;

namespace TaskManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/tasks/{taskId:guid}/status")]
[Authorize]
public sealed class TaskStatusController : ControllerBase
{
    private readonly TaskService _tasks;
    
    public TaskStatusController(TaskService tasks)
    {
        _tasks = tasks;
    }

    [HttpPut]
    public async Task<IActionResult> Change(Guid taskId, ChangeTaskStatusRequestDto request)
    {
        var currentUserId = User.GetUserId();
        
        await _tasks.ChangeStatusAsync(taskId, currentUserId, request);
        return NoContent();
    }
}