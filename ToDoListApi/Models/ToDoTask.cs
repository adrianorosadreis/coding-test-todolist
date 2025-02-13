using System.ComponentModel.DataAnnotations;
using ToDoListApi.Enums;

namespace ToDoListApi.Models
{
    public class ToDoTask
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "O título da tarefa deve ter no máximo 50 caracteres.")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "A descrição da tarefa deve ter no máximo 500 caracteres.")]
        public string Description { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public int CreatedByUserId { get; set; } // Relacionamento com a tabela Users

        public TasksStatus Status { get; set; } // Exemplo: "Em andamento", "Concluída"
    }
}