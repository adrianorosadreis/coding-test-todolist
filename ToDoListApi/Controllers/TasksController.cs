using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDoListApi.Data;
using ToDoListApi.Enums;
using ToDoListApi.Models;

namespace ToDoListApi.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TasksController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoTask>> GetTaskById(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var task = await _taskService.GetTaskById(id, userId);

                if (task == null)
                {
                    return NotFound("Tarefa não encontrada.");
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao buscar a tarefa.", details = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoTask>>> GetTasks([FromQuery] string? status, [FromQuery] int? createdByUserId, [FromQuery] DateTime? dueDate)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var tasks = await _taskService.GetTasks(status, userId, dueDate);

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao listar as tarefas.", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ToDoTask>> CreateTask(ToDoTask task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                task.CreatedByUserId = userId;

                var createdTask = await _taskService.CreateTask(task);

                return CreatedAtAction(nameof(GetTasks), new { id = createdTask.Id }, createdTask);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao criar a tarefa.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, ToDoTask task)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var updatedTask = await _taskService.UpdateTask(id, task, userId);

                if (updatedTask == null)
                {
                    return NotFound("Tarefa não encontrada.");
                }

                return Ok(updatedTask);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao atualizar a tarefa.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _taskService.DeleteTask(id, userId);

                if (!result)
                {
                    return NotFound("Tarefa não encontrada.");
                }

                return Ok("Tarefa removida com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao remover a tarefa.", details = ex.Message });
            }
        }
    }
}