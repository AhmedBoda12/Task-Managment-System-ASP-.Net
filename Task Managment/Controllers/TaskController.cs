using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_Managment.bll.DTOs;
using Task_Managment.bll.Interfaces;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {

        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(string id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                    return NotFound();
                return Ok(task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var tasks = await _taskService.GetAllTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTasks([FromQuery] string keyword, [FromQuery] bool? isCompleted)
        {
            try
            {
                var tasks = await _taskService.SearchTasksAsync(keyword, isCompleted);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] TaskDTO dto)
        {
            try
            {
                var createdTask = await _taskService.AddTaskAsync(dto);
                return CreatedAtAction(nameof(GetTaskById), new { Id = createdTask.Title }, createdTask);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(string id, [FromForm] EditTaskDTO taskDto)
        {
            try
            {
                var updatedTask = await _taskService.UpdateTaskAsync(id, taskDto);
                if (updatedTask == null)
                    return NotFound();
                return Ok("Edited Successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            try
            {
                await _taskService.DeleteTaskAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("Count")]
        public async Task<IActionResult> CountAsync()
        {
            try
            {
                return Ok(await _taskService.CountAsync());
            } catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
