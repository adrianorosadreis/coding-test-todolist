using Microsoft.EntityFrameworkCore;
using System.Text;
using ToDoListApi.Data;
using ToDoListApi.Helpers;
using ToDoListApi.Models;


namespace ToDoListApi.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para listar todos os usuários
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // Método para registrar um novo usuário com senha criptografada
        public async Task<bool> RegisterUser(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                return false; // Usuário já existe

            user.PasswordHash = PasswordHelper.HashPassword(user.PasswordHash); // Criptografar senha antes de salvar
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // Método para autenticar o usuário
        public async Task<User?> Authenticate(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return null;

            // Comparação correta da senha usando SHA256
            var hashedPassword = PasswordHelper.HashPassword(password);
            if (user.PasswordHash != hashedPassword)
                return null;

            return user; // Usuário autenticado com sucesso
        }

        // Método para excluir um usuário
        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false; // Usuário não encontrado

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true; // Usuário removido com sucesso
        }
    }
}