using Microsoft.EntityFrameworkCore;
using ToDoListApi.Data;
using ToDoListApi.Enums;
using ToDoListApi.Models;

public class TaskService
{
    private readonly ApplicationDbContext _context;

    public TaskService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ToDoTask> GetTaskById(int id, int userId)
    {
        return await _context.ToDoTasks
            .FirstOrDefaultAsync(t => t.Id == id && t.CreatedByUserId == userId);
    }

    public async Task<IEnumerable<ToDoTask>> GetTasks(string? status, int userId, DateTime? dueDate)
    {
        var tasksQuery = _context.ToDoTasks.AsQueryable();

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<TasksStatus>(status, true, out var parsedStatus))
        {
            tasksQuery = tasksQuery.Where(t => t.Status == parsedStatus);
        }

        tasksQuery = tasksQuery.Where(t => t.CreatedByUserId == userId);

        if (dueDate.HasValue)
        {
            tasksQuery = tasksQuery.Where(t => t.DueDate.Date == dueDate.Value.Date);
        }

        return await tasksQuery.ToListAsync();
    }

    public bool ValidateTask(ToDoTask task, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (string.IsNullOrEmpty(task.Title))
        {
            errorMessage = "O título da tarefa é obrigatório.";
            return false;
        }

        if (task.Title.Length > 50)
        {
            errorMessage = "O título da tarefa deve ter no máximo 50 caracteres.";
            return false;
        }

        if (task.Description.Length > 500)
        {
            errorMessage = "A descrição da tarefa deve ter no máximo 500 caracteres.";
            return false;
        }

        if (task.DueDate < task.CreatedAt)
        {
            errorMessage = "A data de término não pode ser anterior à data de cadastro.";
            return false;
        }

        return true;
    }

    public async Task<ToDoTask> CreateTask(ToDoTask task)
    {
        if (!ValidateTask(task, out string errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }

        _context.ToDoTasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<ToDoTask?> UpdateTask(int id, ToDoTask task, int userId)
    {
        var existingTask = await _context.ToDoTasks.FindAsync(id);
        if (existingTask == null || existingTask.CreatedByUserId != userId)
        {
            return null; // Retorna null caso a tarefa não exista ou o usuário não tenha permissão
        }

        existingTask.Title = task.Title;
        existingTask.Description = task.Description;
        existingTask.DueDate = task.DueDate;
        existingTask.Status = task.Status;

        _context.Entry(existingTask).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return existingTask;
    }

    public async Task<bool> DeleteTask(int id, int userId)
    {
        var task = await _context.ToDoTasks.FindAsync(id);
        if (task == null || task.CreatedByUserId != userId)
        {
            return false; // Retorna false se a tarefa não existir ou o usuário não tiver permissão
        }

        _context.ToDoTasks.Remove(task);
        await _context.SaveChangesAsync();

        return true;
    }
}