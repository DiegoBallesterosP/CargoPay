using CargoPay.Dtos;

namespace CargoPay.Interfaces
{
    public interface IAuthService
    {
        Task<string> Authenticate(LoginDto loginDto);
    }
}