using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using ToDoListApi.Data;
using ToDoListApi.Helpers;
using ToDoListApi.Models;
using ToDoListApi.Services;

namespace ToDoListApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(ApplicationDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Verifica se o ModelState é válido antes de processar
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);

                if (user == null || !PasswordHelper.VerifyPassword(request.Password, user.PasswordHash))
                {
                    return Unauthorized(new { message = "Usuário ou senha inválidos." });
                }

                var token = _jwtService.GenerateToken(user.Id, user.Username);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao realizar login.", details = ex.Message });
            }
        }
    }
}