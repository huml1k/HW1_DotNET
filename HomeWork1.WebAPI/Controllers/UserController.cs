using HomeWork1.Contracts;
using HomeWork1.WebAPI.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace HomeWork1.WebAPI.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private static readonly Dictionary<string, User> _users = new Dictionary<string, User>();

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationContract request)
        {
            if (_users.ContainsKey(request.Username))
                return Conflict("Пользователь уже существует");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                Password = request.Password 
            };

            _users.Add(request.Username, user);

            return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginContract request)
        {
            if (!_users.ContainsKey(request.Username) ||
                _users[request.Username].Password != request.Password)
                return Unauthorized("Неверные учетные данные");

            return Ok(new { Message = "Успешная авторизация" });
        }


        [HttpPost("{username}")]
        public async Task<IActionResult> UpdateUser(string username, [FromBody] UserUpdateContact request)
        {
            if (!_users.ContainsKey(username))
                return NotFound("Пользователь не найден");

            var user = _users[username];
            user.Email = request.Email;
            user.Password = request.Password;
            user.Username = request.Username;

            return Ok(user);
        }

        // DELETE api/users/{username}
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            if (!_users.ContainsKey(username))
                return NotFound("Пользователь не найден");

            _users.Remove(username);
            return Ok("Пользователь удален");
        }
    }
}
