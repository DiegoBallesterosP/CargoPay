using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CargoPay.Dtos;
using CargoPay.Interfaces;
using FluentValidation;

namespace CargoPay.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<LoginDto> _userValidator;

        public AuthController(IAuthService authService, IValidator<LoginDto> userValidator)
        {
            _authService = authService;
            _userValidator = userValidator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUsers([FromBody] List<LoginDto> userDtos)
        {
            var results = new List<string>();

            foreach (var userDto in userDtos)
            {
                var validationResult = await _userValidator.ValidateAsync(userDto);
                if (!validationResult.IsValid)
                {
                    results.Add($"Validation failed for {userDto.Username}: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}");
                    continue;
                }

                try
                {
                    var result = await _authService.CreateUser(userDto);
                    results.Add(result);
                }
                catch (InvalidOperationException ex)
                {
                    results.Add($"Error creating user {userDto.Username}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    results.Add($"Error creating user {userDto.Username}: {ex.Message}");
                }
            }

            if (results.Any(r => r.StartsWith("Error")))
                return BadRequest(results);

            return Ok(results);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var validationResult = await _userValidator.ValidateAsync(loginDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var token = await _authService.Authenticate(loginDto);
                if (token == null)
                {
                    return Unauthorized("Invalid credentials");
                }

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during login: {ex.Message}");  // Handle internal server error
            }
        }
    }
}