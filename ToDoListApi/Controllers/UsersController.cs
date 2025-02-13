using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListApi.Data;
using ToDoListApi.Models;
using ToDoListApi.Services;

namespace ToDoListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                var users = await _userService.GetUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao listar os usuários.", details = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _userService.RegisterUser(user);
                if (result)
                {
                    return Ok("Usuário registrado com sucesso!");
                }
                return BadRequest("Falha ao registrar usuário.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao registrar o usuário.", details = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _userService.Authenticate(request.Username, request.Password);
                if (user == null)
                {
                    return Unauthorized(new { message = "Usuário ou senha inválidos." });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao realizar login.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUser(id);
                if (!result)
                {
                    return NotFound("Usuário não encontrado.");
                }

                return Ok("Usuário removido com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao remover o usuário.", details = ex.Message });
            }
        }
    }
}