using LibraryApi.Data;
using LibraryApi.Dtos;
using LibraryApi.Helpers;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly LibraryContext _context;

        public AuthController(LibraryContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Username == loginDto.Username);

            if (admin == null)
                return Unauthorized(new { message = "Usuario no encontrado." });

            bool passwordValid = PasswordHelper.VerifyPassword(admin.PasswordHash, loginDto.Password);
            if (!passwordValid)
                return Unauthorized(new { message = "Contraseña incorrecta." });

            return Ok(new { message = "Inicio de sesión exitoso" });
        }
    }
}
