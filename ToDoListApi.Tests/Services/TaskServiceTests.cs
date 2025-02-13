using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListApi.Data;
using ToDoListApi.Enums;
using ToDoListApi.Models;

namespace ToDoListApi.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly TaskService _taskService;
        private readonly ApplicationDbContext _context;

        public TaskServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _taskService = new TaskService(_context);
        }

        [Fact]
        public void ValidateTask_ShouldReturnFalse_WhenTitleIsEmpty()
        {
            var task = new ToDoTask
            {
                Title = "",
                Description = "Description",
                DueDate = DateTime.Now.AddDays(1)
            };

            var result = _taskService.ValidateTask(task, out string errorMessage);

            Assert.False(result);
            Assert.Equal("O título da tarefa é obrigatório.", errorMessage);
        }

        [Fact]
        public void ValidateTask_ShouldReturnTrue_WhenTaskIsValid()
        {
            var task = new ToDoTask
            {
                Title = "Valid Task",
                Description = "Description",
                DueDate = DateTime.Now.AddDays(1)
            };

            var result = _taskService.ValidateTask(task, out string errorMessage);

            Assert.True(result);
            Assert.Empty(errorMessage);
        }

        [Fact]
        public async Task CreateTask_ShouldCreateTask_WhenValid()
        {
            var task = new ToDoTask
            {
                Title = "Test Task",
                Description = "Description",
                DueDate = DateTime.Now.AddDays(1),
                Status = TasksStatus.Pendente,
                CreatedByUserId = 1
            };

            _context.ToDoTasks.Add(task);
            await _context.SaveChangesAsync();

            var createdTask = await _context.ToDoTasks.FirstOrDefaultAsync(t => t.Title == "Test Task");

            Assert.NotNull(createdTask);
        }

        [Fact]
        public async Task UpdateTask_ShouldUpdateTask_WhenValid()
        {
            var task = new ToDoTask
            {
                Title = "Old Task",
                Description = "Old Description",
                DueDate = DateTime.Now.AddDays(1),
                Status = TasksStatus.Pendente,
                CreatedByUserId = 1
            };

            _context.ToDoTasks.Add(task);
            await _context.SaveChangesAsync();

            task.Title = "Updated Task";
            task.Description = "Updated Description";

            _context.ToDoTasks.Update(task);
            await _context.SaveChangesAsync();

            var updatedTask = await _context.ToDoTasks.FirstOrDefaultAsync(t => t.Id == task.Id);

            Assert.Equal("Updated Task", updatedTask?.Title);
            Assert.Equal("Updated Description", updatedTask?.Description);
        }

        [Fact]
        public async Task DeleteTask_ShouldDeleteTask_WhenValid()
        {
            var task = new ToDoTask
            {
                Title = "Test Task",
                Description = "Description",
                DueDate = DateTime.Now.AddDays(1),
                Status = TasksStatus.Pendente,
                CreatedByUserId = 1
            };

            _context.ToDoTasks.Add(task);
            await _context.SaveChangesAsync();

            _context.ToDoTasks.Remove(task);
            await _context.SaveChangesAsync();

            var deletedTask = await _context.ToDoTasks.FindAsync(task.Id);

            Assert.Null(deletedTask);
        }

        [Fact]
        public async Task CreateTask_ShouldReturnTask_WhenTaskIsCreated()
        {
            var task = new ToDoTask
            {
                Title = "Nova Tarefa",
                Description = "Descrição da Tarefa",
                Status = TasksStatus.Pendente,
                Id = 999 // usuario invalido
            };

            var createdTask = await _taskService.CreateTask(task);

            Assert.NotNull(createdTask);
            Assert.Equal("Nova Tarefa", createdTask.Title);
        }
    }
}
