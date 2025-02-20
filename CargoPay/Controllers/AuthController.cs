using CargoPay.Dtos;
using CargoPay.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CargoPay.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _authService.Authenticate(loginDto);
            if (token == null)
            {
                return Unauthorized("Credenciales inválidas");
            }

            return Ok(new { token });
        }
    }
}