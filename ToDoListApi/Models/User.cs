using System.ComponentModel.DataAnnotations;

namespace ToDoListApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres.")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "O nome deve conter apenas letras.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(30, ErrorMessage = "O username deve ter no máximo 30 caracteres.")]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9._]{0,29}$", ErrorMessage = "O username deve ser alfanumérico, com ponto ou underline, e começar com letra.")]
        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;
    }
}