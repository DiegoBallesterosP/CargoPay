using CargoPay.Entities;

namespace CargoPay.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAndPassword(string username, string password);
    }
}