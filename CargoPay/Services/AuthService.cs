using CargoPay.Dtos;
using CargoPay.Entities;
using CargoPay.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CargoPay.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthService(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<string> Authenticate(LoginDto loginDto)
        {
            try
            {
                var hashedPassword = HashPassword(loginDto.Password);
                var user = await _userRepository.GetUserByUsernameAndPassword(loginDto.Username.ToLower(), hashedPassword);
                if (user == null)
                    return null;

                return GenerateJwtToken(user.Username);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error during authentication.", ex);
            }
        }

        public async Task<string> CreateUser(LoginDto userDto)
        {
            try
            {
                if (await _userRepository.UserExists(userDto.Username))
                    throw new InvalidOperationException("Username already exists.");

                var hashedPassword = HashPassword(userDto.Password);
                var user = new User
                {
                    Username = userDto.Username.ToLower(),
                    Password = hashedPassword
                };

                await _userRepository.AddUser(user);
                return "User created successfully.";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error creating user.", ex);
            }
        }

        private string GenerateJwtToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hash.Substring(0, 30);
            }
        }
    }
}
